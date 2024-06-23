namespace User.Application.Feature.v1.Users.Queries.GetUserQuery;

public class GetUserQueryHandler(IUserRepository userRepository, IApiExternalClient externalClient) : IRequestHandler<GetUserQuery, ApiResult<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    private readonly IApiExternalClient _externalClient = externalClient;
    public async Task<ApiResult<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        UserDto? userResult = await _userRepository.GetUserByInput(request.Input, cancellationToken);
        if (userResult is null)
        {
            return new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound);
        }
        ExternalApiResponse<AccountInfoModel>? accountResponse = await _externalClient.GetAccount(userResult.AccountGuid, cancellationToken);

        if (accountResponse.Data is null)
        {
            return new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound);
        }
        UserDto result = new()
        {
            Id = userResult.Id,
            FirstName = userResult.FirstName,
            LastName = userResult.LastName,
            PhoneNumber = userResult.PhoneNumber,
            EmailAddress = userResult.EmailAddress,
            AvatarUrl = userResult.AvatarUrl,
            Address = userResult.Address,
            ProfileImages = userResult.ProfileImages,
            Longitude = userResult.Longitude,
            Latitude = userResult.Latitude,
            Gender = userResult.Gender,
            BirthDate = userResult.BirthDate,
            AccountGuid = userResult.AccountGuid,
            CategoryIds = userResult.CategoryIds,
            LastModifiedDate = accountResponse.Data.LastModifiedDate.TimeStampToDateTime(),
        };
        return new ApiSuccessResult<UserDto>(result);
    }
}
