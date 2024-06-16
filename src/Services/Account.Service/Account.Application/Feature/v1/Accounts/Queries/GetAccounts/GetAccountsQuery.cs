namespace Account.Application.Feature.v1.Accounts.Queries.GetAccounts;

public class GetAccountsQuery : IRequest<ApiResult<IEnumerable<GetAccountsQueryResponse>>>
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}