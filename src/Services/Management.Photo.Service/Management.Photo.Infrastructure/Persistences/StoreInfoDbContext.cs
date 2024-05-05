using Contracts.Domains.Interfaces;
using Infrastructure.Extensions;
using Management.Photo.Domain.Entities;
using Management.Photo.Infrastructure.Persistences.EntitiesConfigure;
using Microsoft.EntityFrameworkCore;

namespace Management.Photo.Infrastructure.Persistences
{
    public class StoreInfoDbContext(DbContextOptions<StoreInfoDbContext> options) : DbContext(options)
    {
        public DbSet<StoreInfoEntity> StoreInfoEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _ = modelBuilder.ApplyConfiguration(new StoreInfoConfigure());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> modifiedEntries = ChangeTracker
                .Entries()
                .Where(x => x.State is EntityState.Added
                or EntityState.Modified
                or EntityState.Deleted);
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry? item in modifiedEntries)
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
}