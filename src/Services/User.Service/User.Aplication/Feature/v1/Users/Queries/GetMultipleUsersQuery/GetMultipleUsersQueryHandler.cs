namespace User.Application.Feature.v1.Users.Queries.GetMultipleUsersQuery;

public class GetMultipleUsersQueryHandler(IUserRepository userRepository, IApiExternalClient externalClient) : IRequestHandler<GetMultipleUsersQuery, ApiResult<IEnumerable<UserDto>>>
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    private readonly IApiExternalClient _externalClient = externalClient;

    public async Task<ApiResult<IEnumerable<UserDto>>> Handle(GetMultipleUsersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<UserDto>? userResult = await _userRepository.GetUsersByInput(request.Input, request.PageIndex, request.PageSize, cancellationToken);

        if (userResult is null || !userResult!.Any())
        {
            return new ApiErrorResult<IEnumerable<UserDto>>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound);
        }
        string accountId = string.Join(',', userResult.Select(x => x.AccountGuid.ToString()));
        ExternalApiResponse<IEnumerable<AccountInfoModel>>? accountsResponse = await _externalClient.GetAccounts(accountId, cancellationToken);

        if (!accountsResponse.Data.Any())
        {
            return new ApiErrorResult<IEnumerable<UserDto>>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound);
        }
        IEnumerable<UserDto>? result = userResult.Select(x => new UserDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            PhoneNumber = x.PhoneNumber,
            EmailAddress = x.EmailAddress,
            AvatarUrl = x.AvatarUrl,
            Address = x.Address,
            ProfileImages = x.ProfileImages,
            Longitude = x.Longitude,
            Latitude = x.Latitude,
            Gender = x.Gender,
            BirthDate = x.BirthDate,
            AccountGuid = x.AccountGuid,
            CategoryIds = x.CategoryIds,
            LastModifiedDate = accountsResponse.Data.FirstOrDefault(x => x.AccountId == x.AccountId)!.LastModifiedDate.ToDateTime(),
        });
        return result is null
            ? new ApiErrorResult<IEnumerable<UserDto>>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<IEnumerable<UserDto>>(userResult);
    }
}