namespace Account.Application.Infrastructure.feature.v1.Accounts.Queries.Base;

public abstract class AccountCommandResponseBase
{
    public Guid AccountId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}