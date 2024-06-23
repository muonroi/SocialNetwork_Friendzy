namespace Account.Application.Feature.v1.Accounts.Queries.GetAccounts;

public class GetAccountsQueryResponse : AccountQueryResponseBase
{
    public long LastModifiedDate { get; set; }
}