namespace Management.Friends.Action.Application.Feature.v1.Query.GetFriendByUserIdQuery
{
    public class GetFriendByUserIdQuery : IRequest<ApiResult<IEnumerable<GetFriendByUserIdQueryResponse>>>
    {
        public long UserId { get; set; }
        public ActionMatched ActionMatched { get; set; }
    }
}
