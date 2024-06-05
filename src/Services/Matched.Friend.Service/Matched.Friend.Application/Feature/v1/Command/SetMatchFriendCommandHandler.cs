namespace Matched.Friend.Application.Feature.v1.Command;

public class SetMatchFriendCommandHandler(
    IWorkContextAccessor workContextAccessor,
    ILogger logger,
    IFriendsMatchedRepository friendMatchedRepository,
    IApiExternalClient externalClient) : IRequestHandler<SetMatchFriendCommand, ApiResult<bool>>
{
    private readonly ILogger _logger = logger;

    private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor;

    private readonly IFriendsMatchedRepository _friendMatchedRepository = friendMatchedRepository;

    private readonly IApiExternalClient _externalClient = externalClient;

    public async Task<ApiResult<bool>> Handle(SetMatchFriendCommand request, CancellationToken cancellationToken)
    {
        WorkContextInfoDTO userInfo = _workContextAccessor.WorkContext!;
        _logger.Information($"BEGIN: SetMatchFriendCommandHandler REQUEST --> {JsonConvert.SerializeObject(request)} <--");
        ExternalApiResponse<UserDataDTO> friendInfoResult = await _externalClient.GetUserAsync(request.FriendId.ToString(), cancellationToken);
        if (friendInfoResult.Data is null)
        {
            return new ApiErrorResult<bool>(nameof(ErrorMessageBase.UserNotFound), (int)HttpStatusCode.NotFound);
        }

        bool isFriendMatchedByAction = await _friendMatchedRepository.IsExistFriendAction(userInfo.UserId, request.FriendId, request.ActionMatched, cancellationToken);

        if (!isFriendMatchedByAction)
        {
            return new ApiErrorResult<bool>(nameof(ErrorMessages.UserIsMatched), (int)HttpStatusCode.Conflict, arguments: request.ActionMatched.ToString());
        }

        bool setResult = await _friendMatchedRepository.SetMatchFriendByAction(userInfo.UserId, request.FriendId, request.ActionMatched, cancellationToken).ConfigureAwait(false);

        ApiSuccessResult<bool> result = new(setResult);

        _logger.Information($"END: SetMatchFriendCommandHandler RESULT --> {JsonConvert.SerializeObject(result)} <--");
        return result;
    }
}