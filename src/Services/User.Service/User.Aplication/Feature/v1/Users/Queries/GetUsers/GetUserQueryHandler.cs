namespace User.Application.Feature.v1.Users.Queries.GetUsers;

public class GetUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersQuery, ApiResult<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    public async Task<ApiResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        UserDto? result = await _userRepository.GetUserByInput(request.Input, cancellationToken);
        return result is null
            ? new ApiErrorResult<UserDto>($"{UserErrorMessages.UserNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<UserDto>(result);
    }
}