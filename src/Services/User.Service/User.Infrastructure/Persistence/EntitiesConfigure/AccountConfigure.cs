namespace User.Infrastructure.Persistence.EntitiesConfigure;

public class AccountConfigure : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> modelBuilder)
    {
        _ = modelBuilder
            .HasMany(e => e.UserEntity)
            .WithOne(e => e.AccountEntity)
            .HasForeignKey(e => e.AccountGuid)
            .IsRequired();
    }
}