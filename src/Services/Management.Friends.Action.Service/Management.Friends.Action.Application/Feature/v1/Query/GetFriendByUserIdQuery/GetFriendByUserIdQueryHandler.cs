namespace Management.Friends.Action.Application.Feature.v1.Query.GetFriendByUserIdQuery;

public class GetFriendByUserIdQueryHandler(IFriendsActionRepository friendMatchedRepository,
        IWorkContextAccessor workContextAccessor
        , IApiExternalClient externalClient
        , ILogger logger
        , ISerializeService serializeService) : IRequestHandler<GetFriendByUserIdQuery, ApiResult<IEnumerable<GetFriendByUserIdQueryResponse>>>
{
    private readonly IFriendsActionRepository _friendMatchedRepository = friendMatchedRepository;

    private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor;

    private readonly ILogger _logger = logger;

    private readonly IApiExternalClient _externalClient = externalClient;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<ApiResult<IEnumerable<GetFriendByUserIdQueryResponse>>> Handle(GetFriendByUserIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<FriendsActionDto>? friendIdResult = await _friendMatchedRepository.GetFriendsById(request.UserId, request.ActionMatched, cancellationToken);

        if (friendIdResult is null)
        {
            return new ApiErrorResult<IEnumerable<GetFriendByUserIdQueryResponse>>(nameof(ErrorMessages.FriendsMatchedsNotFound), (int)HttpStatusCode.NotFound, arguments: request.ActionMatched.ToString());
        }

        IEnumerable<GetFriendByUserIdQueryResponse> result = friendIdResult.Select(x => new GetFriendByUserIdQueryResponse
        {
            FriendId = x.FriendId,
            ActionMatched = (int)x.ActionMatched
        });
        return new ApiSuccessResult<IEnumerable<GetFriendByUserIdQueryResponse>>(result);
    }
}