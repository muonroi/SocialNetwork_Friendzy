using Contracts.Domains.Interfaces;
using Infrastructure.Extensions;
using Management.Photo.Domain.Entities;
using Management.Photo.Infrastructure.Persistance.EntitiesConfigure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Management.Photo.Infrastructure.Persistance;

public class StoreInfoDbContext(DbContextOptions<StoreInfoDbContext> options) : DbContext(options)
{
    public DbSet<StoreInfoEntity> StoreInfoEntities { get; set; }
    public DbSet<BucketEntity> BucketEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _ = modelBuilder.ApplyConfiguration(new StoreInfoConfigure());
        _ = modelBuilder.ApplyConfiguration(new BucketConfiguration());
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