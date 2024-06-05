namespace Matched.Friend.Application.Feature.v1.Query.GetMatchedFriendsByUserQuery;

public class GetFriendsMatchedByUserQuery : IRequest<ApiResult<PagingResponse<IEnumerable<GetFriendsMatchedByUserQueryResponse>>>>
{
    public ActionMatched ActionMatched { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}