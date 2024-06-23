namespace Account.Application.Feature.v1.Accounts.Queries.GetAccount;

public class GetAccountQueryResponse : AccountQueryResponseBase
{
    public long LastModifiedDate { get; set; }
}