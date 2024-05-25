using Management.Photo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Management.Photo.Infrastructure.Persistances.EntitiesConfigure;

public class StoreInfoConfigure : IEntityTypeConfiguration<StoreInfoEntity>
{
    public void Configure(EntityTypeBuilder<StoreInfoEntity> modelBuilder)
    {
        _ = modelBuilder.Property(x => x.StoreUrl).IsUnicode(false).HasMaxLength(1000).IsRequired();
        _ = modelBuilder.Property(x => x.StoreDescription).HasMaxLength(1000);
        _ = modelBuilder.Property(x => x.StoreName).HasMaxLength(255).IsRequired();
        _ = modelBuilder.Property(x => x.UserId).IsRequired();
        _ = modelBuilder.Property(x => x.BucketId).IsRequired();
    }
}