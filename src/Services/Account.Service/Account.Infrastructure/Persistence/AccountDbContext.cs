namespace Account.Infrastructure.Persistence;

public class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options)
{
    public DbSet<AccountEntity> Accounts { get; set; }

    public DbSet<AccountRolesEntity> AccountRoles { get; set; }

    public DbSet<RoleEntity> RoleEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _ = modelBuilder.ApplyConfiguration(new AccountConfigure());
        _ = modelBuilder.ApplyConfiguration(new AccountRoleConfigure());
        _ = modelBuilder.ApplyConfiguration(new RoleConfigure());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        IEnumerable<EntityEntry> modifiedEntries = ChangeTracker
            .Entries()
            .Where(x => x.State is EntityState.Added
            or EntityState.Modified
            or EntityState.Deleted);
        foreach (EntityEntry? item in modifiedEntries)
        {
            switch (item.State)
            {
                case EntityState.Added:
                    if (item.Entity is IDateTracking addedEntity)
                    {
                        addedEntity.CreatedDate = DateTime.UtcNow;
                        addedEntity.CreatedDateTs = DateTime.UtcNow.GetTimeStamp();
                        item.State = EntityState.Added;
                    }
                    break;

                case EntityState.Modified:
                    Entry(item.Entity).Property("Id").IsModified = false;
                    if (item.Entity is IDateTracking modifiedEntity)
                    {
                        modifiedEntity.LastModifiedDate = DateTime.UtcNow;
                        modifiedEntity.LastModifiedDateTs = DateTime.UtcNow.GetTimeStamp();
                        item.State = EntityState.Modified;
                    }
                    break;

                case EntityState.Detached:
                    break;

                case EntityState.Unchanged:
                    break;

                case EntityState.Deleted:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}