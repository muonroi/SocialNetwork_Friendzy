using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.EntitiesConfigure
{
    public class AccountRoleConfigure : IEntityTypeConfiguration<AccountRolesEntity>
    {
        public void Configure(EntityTypeBuilder<AccountRolesEntity> modelBuilder)
        {
            _ = modelBuilder
          .HasKey(accountRole => new { accountRole.AccountId, accountRole.RoleId });

            _ = modelBuilder
                .HasOne(ur => ur.Account)
                .WithMany(u => u.AccountRoles)
                .HasForeignKey(ur => ur.AccountId);

            _ = modelBuilder
                .HasOne(ur => ur.Role)
                .WithMany(r => r.AccountRoles)
                .HasForeignKey(ur => ur.RoleId);
        }
    }
}