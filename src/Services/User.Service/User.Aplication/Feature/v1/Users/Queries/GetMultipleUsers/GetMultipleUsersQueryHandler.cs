namespace User.Application.Feature.v1.Users.Queries.GetMultipleUsers;

public class GetMultipleUsersQueryHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<GetMultipleUsersQuery, ApiResult<IEnumerable<UserDto>>>
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    public async Task<ApiResult<IEnumerable<UserDto>>> Handle(GetMultipleUsersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<UserDto>? result = await _userRepository.GetUsersByInput(request.Input, cancellationToken);
        return result is null
            ? new ApiErrorResult<IEnumerable<UserDto>>($"{UserErrorMessages.UserNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<IEnumerable<UserDto>>(result);
    }
}