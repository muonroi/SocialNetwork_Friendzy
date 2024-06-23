namespace Account.Application.Feature.v1.Accounts.Queries.GetAccountsPaging;

public class GetAccountsPagingQuery : IRequest<ApiResult<IEnumerable<GetAccountsPagingQueryResponse>>>
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}