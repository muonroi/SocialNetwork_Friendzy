namespace Account.Application.Infrastructure.feature.v1.Accounts.Queries.Base;

public abstract class AccountQueryResponseBase
{
    public Guid AccountId { get; set; }

    public AccountType AccountType { get; set; }

    public Currency Currency { get; set; }

    public string LockReason { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public bool IsActive { get; set; }

    public bool IsEmailVerified { get; set; }

    public AccountStatus Status { get; set; }
    public string Roles { get; set; } = string.Empty;
}