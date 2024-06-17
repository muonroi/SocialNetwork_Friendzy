namespace Management.Friends.Action.Application.Feature.v1.Query.GetFriendsByIdQuery;

public class GetFriendsByIdQueryHandler(IFriendsActionRepository friendMatchedRepository,
        IWorkContextAccessor workContextAccessor
        , IApiExternalClient externalClient
        , ILogger logger
        , ISerializeService serializeService) : IRequestHandler<GetFriendsByIdQuery, ApiResult<IEnumerable<GetFriendsByIdQueryResponse>>>
{
    private readonly IFriendsActionRepository _friendMatchedRepository = friendMatchedRepository;

    private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor;

    private readonly ILogger _logger = logger;

    private readonly IApiExternalClient _externalClient = externalClient;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<ApiResult<IEnumerable<GetFriendsByIdQueryResponse>>> Handle(GetFriendsByIdQuery request, CancellationToken cancellationToken)
    {
        // WorkContextInfoDTO workContext = _workContextAccessor.WorkContext!;

        IEnumerable<Commons.Models.FriendsActionDto>? friendIdResult = await _friendMatchedRepository.GetFriendsById(request.UserId, request.FriendIds, request.ActionMatched, cancellationToken);
        if (friendIdResult is null)
        {
            return new ApiErrorResult<IEnumerable<GetFriendsByIdQueryResponse>>(nameof(ErrorMessages.FriendsMatchedsNotFound), (int)HttpStatusCode.NotFound, arguments: request.ActionMatched.ToString());
        }
        IEnumerable<GetFriendsByIdQueryResponse> result = friendIdResult.Select(x => new GetFriendsByIdQueryResponse
        {
            FriendId = x.FriendId,
            ActionMatched = (int)x.ActionMatched
        });
        return new ApiSuccessResult<IEnumerable<GetFriendsByIdQueryResponse>>(result);
    }
}