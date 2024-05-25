namespace User.Application.Feature.v1.Users.Queries.GetMultipleUsersQuery;

public class GetMultipleUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetMultipleUsersQuery, ApiResult<IEnumerable<UserDto>>>
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    public async Task<ApiResult<IEnumerable<UserDto>>> Handle(GetMultipleUsersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<UserDto>? result = await _userRepository.GetUsersByInput(request.Input, cancellationToken);
        return result is null
            ? new ApiErrorResult<IEnumerable<UserDto>>($"{UserErrorMessages.UserNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<IEnumerable<UserDto>>(result);
    }
}