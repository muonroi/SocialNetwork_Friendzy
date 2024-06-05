namespace Account.Domain.Entities;

public class AccountRolesEntity : EntityAuditBase<Guid>
{
    public Guid AccountId { get; set; }
    public Guid RoleId { get; set; }
    public AccountEntity? Account { get; set; }
    public RoleEntity? Role { get; set; }
}