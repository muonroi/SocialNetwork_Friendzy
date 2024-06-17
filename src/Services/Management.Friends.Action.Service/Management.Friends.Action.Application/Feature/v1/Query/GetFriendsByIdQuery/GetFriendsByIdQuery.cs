namespace Management.Friends.Action.Application.Feature.v1.Query.GetFriendsByIdQuery;

public class GetFriendsByIdQuery : IRequest<ApiResult<IEnumerable<GetFriendsByIdQueryResponse>>>
{
    public IEnumerable<long> FriendIds { get; set; } = [];
    public long UserId { get; set; }
    public ActionMatched ActionMatched { get; set; }
}