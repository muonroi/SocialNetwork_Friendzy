namespace Management.Friends.Action.Application.Feature.v1.Query.GetFriendsActionByUserQuery;

public class GetFriendsActionByUserQuery : IRequest<ApiResult<PagingResponse<IEnumerable<GetFriendsActionByUserQueryResponse>>>>
{
    public ActionMatched ActionMatched { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}