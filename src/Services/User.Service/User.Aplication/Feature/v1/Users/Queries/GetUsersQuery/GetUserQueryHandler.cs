using Shared.DTOs;

namespace User.Application.Feature.v1.Users.Queries.GetUsersQuery;

public class GetUserQueryHandler(IUserRepository userRepository, IWorkContextAccessor workContextAccessor) : IRequestHandler<GetUserQuery, ApiResult<UserDto>>
{
    private readonly IWorkContextAccessor _workContext = workContextAccessor ?? throw new ArgumentNullException(nameof(workContextAccessor));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    public async Task<ApiResult<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        WorkContextInfoDTO userInfo = _workContext.WorkContext!;
        UserDto? result = await _userRepository.GetUserByInput(string.IsNullOrEmpty(request.Input) ? userInfo.UserId.ToString() : request.Input, cancellationToken);
        return result is null
            ? new ApiErrorResult<UserDto>($"{ErrorMessageBase.UserNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<UserDto>(result);
    }
}