namespace Account.Application.Feature.v1.Accounts.Queries.GetAccount;

public class GetAccountQuery : IRequest<ApiResult<GetAccountQueryResponse>>
{
    public Guid AccountId { get; set; }
}