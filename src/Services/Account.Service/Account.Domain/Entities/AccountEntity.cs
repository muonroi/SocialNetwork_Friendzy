namespace Account.Domain.Entities;

public class AccountEntity : EntityAuditBase<Guid>
{
    public AccountType AccountType { get; set; }

    public Currency Currency { get; set; }

    [Column(TypeName = "nvarchar(256)")]
    public string LockReason { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Balance { get; set; }

    public bool IsActive { get; set; }

    public bool IsEmailVerified { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public double RefreshTokenExpiryTime { get; set; }

    public AccountStatus Status { get; set; }

    public IEnumerable<AccountRolesEntity>? AccountRoles { get; set; }
}