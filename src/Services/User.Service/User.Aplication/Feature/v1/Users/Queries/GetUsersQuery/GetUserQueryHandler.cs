using Shared.Resources;

namespace User.Application.Feature.v1.Users.Queries.GetUsersQuery;

public class GetUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersQuery, ApiResult<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    public async Task<ApiResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        UserDto? result = await _userRepository.GetUserByInput(request.Input, cancellationToken);
        return result is null
            ? new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<UserDto>(result);
    }
}