
using ExternalAPI;
using ExternalAPI.DTOs;
using Shared.Resources;

namespace User.Application.Feature.v1.Users.Commands.UserRegisterCommand
{
    public class UserRegisterCommandHandler(IUserRepository userRepository, IApiExternalClient externalClient) : IRequestHandler<UserRegisterCommand, ApiResult<UserDto>>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        private readonly IApiExternalClient _externalClient = externalClient;

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
                Address = request.Address,
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                CategoryId = request.CategoryId
            };
            _ = await _userRepository.CreateUserByPhone(result, cancellationToken);

            UserDto? userCreated = await _userRepository.GetUserByInput(request.PhoneNumber, cancellationToken);

            if (userCreated is null)
            {
                return new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound);
            }

            CreateAccountDto createAccountDto = new()
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

            ExternalApiResponse<AccountDataDTO>? accountResponse = await _externalClient.CreateAccountAsync(createAccountDto, cancellationToken);

            if (accountResponse.Data is null)
            {
                return new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound);
            }

            result.AccountGuid = accountResponse.Data.AccountId;

            result.AccessToken = accountResponse.Data.AccessToken;

            result.RefreshToken = accountResponse.Data.RefreshToken;

            result.Id = userCreated.Id;

            bool updateUserInfo = await _userRepository.UpdateUserByPhone(result, result.PhoneNumber, cancellationToken);

            return updateUserInfo ? new ApiSuccessResult<UserDto>(result) : new ApiErrorResult<UserDto>();
        }
    }
}
