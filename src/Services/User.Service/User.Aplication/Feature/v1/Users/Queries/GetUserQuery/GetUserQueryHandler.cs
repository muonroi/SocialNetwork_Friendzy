namespace User.Application.Feature.v1.Users.Queries.GetUserQuery;

public class GetUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserQuery, ApiResult<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    public async Task<ApiResult<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        UserDto? result = await _userRepository.GetUserByInput(request.Input, cancellationToken);
        return result is null
            ? new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<UserDto>(result);
    }
}