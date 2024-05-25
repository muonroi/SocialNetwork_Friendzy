using Contracts.Domains;

namespace Account.Domain.Entities
{
    public class RoleEntity : EntityAuditBase<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<AccountRolesEntity>? AccountRoles { get; set; }
    }
}
