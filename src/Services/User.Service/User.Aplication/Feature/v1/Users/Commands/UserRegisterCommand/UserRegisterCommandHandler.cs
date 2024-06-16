using Distance.Service.Protos;
using ExternalAPI.Models;
using User.Application.Messages;
using static Distance.Service.Protos.DistanceService;

namespace User.Application.Feature.v1.Users.Commands.UserRegisterCommand;

public class UserRegisterCommandHandler(GrpcClientFactory grpcClientFactory
    , IUserRepository userRepository, IApiExternalClient externalClient) : IRequestHandler<UserRegisterCommand, ApiResult<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    private readonly IApiExternalClient _externalClient = externalClient;


    private readonly DistanceServiceClient _distanceServiceClient =
        grpcClientFactory.CreateClient<DistanceServiceClient>(ServiceConstants.DistanceService);

    public async Task<ApiResult<UserDto>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        UserDto? result = await _userRepository.GetUserByInput(request.PhoneNumber, cancellationToken);

        if (result is not null)
        {
            return new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserHasRegisted}", (int)HttpStatusCode.Conflict, arguments: request.PhoneNumber);
        }

        result = new UserDto
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            EmailAddress = request.EmailAddress ?? string.Empty,
            AvatarUrl = request.AvatarUrl,
            Birthdate = request.Birthdate,
            ProfileImages = request.ProfileImagesUrl.Split(','),
            Address = request.Address,
            Longitude = request.Longitude,
            Latitude = request.Latitude,
            CategoryIds = request.CategoryId.Split(','),
            Gender = request.Gender
        };
        _ = await _userRepository.CreateUserByPhone(result, cancellationToken);

        UserDto? userCreated = await _userRepository.GetUserByInput(request.PhoneNumber, cancellationToken);

        if (userCreated is null)
        {
            return new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound);
        }

        CreateAccountModel createAccountDto = new()
        {
            AccountType = AccountType.Personal,
            Currency = Currency.VND,
            IsActive = true,
            IsEmailVerified = false,
            Status = AccountStatus.Online,
            Roles = RoleConstants.User.ToString(),
            UserId = userCreated.Id,
            FirstName = userCreated.FirstName,
            LastName = userCreated.LastName,
            PhoneNumber = userCreated.PhoneNumber,
            EmailAddress = userCreated.EmailAddress ?? string.Empty,
            AvatarUrl = userCreated.AvatarUrl,
            Address = userCreated.Address,
            Longitude = userCreated.Longitude,
            Latitude = userCreated.Latitude
        };

        ExternalApiResponse<AccountDataModel>? accountResponse = await _externalClient.CreateAccountAsync(createAccountDto, cancellationToken);

        if (accountResponse.Data is null)
        {
            return new ApiErrorResult<UserDto>($"{UserErrorMessages.CreateAccountError}", (int)HttpStatusCode.NotFound);
        }

        CreateDistanceInfoReply distanceResult = await _distanceServiceClient.CreateDistanceInfoAsync(new CreateDistanceInfoRequest
        {
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Country = "Viet nam",
            UserId = userCreated.Id
        }, cancellationToken: cancellationToken);

        if (!distanceResult.IsSuccess)
        {
            return new ApiErrorResult<UserDto>($"{UserErrorMessages.InsertDistanceError}", (int)HttpStatusCode.NotFound);
        }

        result.AccountGuid = accountResponse.Data.AccountId;

        result.AccessToken = accountResponse.Data.AccessToken;

        result.RefreshToken = accountResponse.Data.RefreshToken;

        result.Id = userCreated.Id;

        bool updateUserInfo = await _userRepository.UpdateUserByPhone(result, result.PhoneNumber, cancellationToken);

        return updateUserInfo ? new ApiSuccessResult<UserDto>(result) : new ApiErrorResult<UserDto>();
    }
}