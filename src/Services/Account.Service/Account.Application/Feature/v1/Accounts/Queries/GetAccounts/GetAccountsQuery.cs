namespace Account.Application.Feature.v1.Accounts.Queries.GetAccounts;

public class GetAccountsQuery : IRequest<ApiResult<IEnumerable<GetAccountsQueryResponse>>>
{
    public string Input { get; set; } = string.Empty;
}