﻿namespace Management.Friends.Action.Application.Feature.v1.Command;

public class SetFriendByActionCommandHandler(
    IWorkContextAccessor workContextAccessor,
    ILogger logger,
    IFriendsActionRepository friendMatchedRepository,
    IApiExternalClient externalClient,
    ISerializeService serializeService) : IRequestHandler<SetFriendByActionCommand, ApiResult<bool>>
{
    private readonly ILogger _logger = logger;

    private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor;

    private readonly IFriendsActionRepository _friendMatchedRepository = friendMatchedRepository;

    private readonly IApiExternalClient _externalClient = externalClient;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<ApiResult<bool>> Handle(SetFriendByActionCommand request, CancellationToken cancellationToken)
    {
        WorkContextInfoModel userInfo = _workContextAccessor.WorkContext!;
        _logger.Information($"BEGIN: SetMatchFriendCommandHandler REQUEST --> {_serializeService.Serialize(request)} <--");
        ExternalApiResponse<UserDataModel> friendInfoResult = await _externalClient.GetUserAsync(request.FriendId.ToString(), cancellationToken);
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

        _logger.Information($"END: SetMatchFriendCommandHandler RESULT --> {_serializeService.Serialize(result)} <--");
        return result;
    }
}