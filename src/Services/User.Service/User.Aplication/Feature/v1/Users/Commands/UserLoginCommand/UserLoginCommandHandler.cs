using ExternalAPI;
using ExternalAPI.DTOs;

namespace User.Application.Feature.v1.Users.Commands.UserLoginCommand
{
    public class UserLoginCommandHandler(IUserRepository userRepository, IApiExternalClient externalClient) : IRequestHandler<UserLoginCommand, ApiResult<UserDto>>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        private readonly IApiExternalClient _externalClient = externalClient;


        public async Task<ApiResult<UserDto>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            UserDto? result = await _userRepository.GetUserByInput(request.PhoneNumber, cancellationToken);
            if (result is null)
            {
                return new ApiErrorResult<UserDto>($"{UserErrorMessages.UserNotFound}", (int)HttpStatusCode.NotFound);
            }
            ExternalApiResponse<AccountDataDTO>? accountResponse = await _externalClient.VerifyAccountAsync(new AccountRequestBase
            {
                Id = result.AccountGuid,
                UserId = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                PhoneNumber = result.PhoneNumber,
                EmailAddress = result.EmailAddress,
                AvatarUrl = result.AvatarUrl,
                Address = result.Address,
                Longitude = result.Longtitude,
                Latitude = result.Latitude
            }, cancellationToken);

            if (accountResponse.Data is null)
            {
                return new ApiErrorResult<UserDto>($"{UserErrorMessages.UserNotFound}", (int)HttpStatusCode.NotFound);
            }
            result.AccountGuid = accountResponse.Data.AccountId;
            result.AccessToken = accountResponse.Data.AccessToken;
            result.RefreshToken = accountResponse.Data.RefreshToken;
            return new ApiSuccessResult<UserDto>(result);
        }
    }
}
