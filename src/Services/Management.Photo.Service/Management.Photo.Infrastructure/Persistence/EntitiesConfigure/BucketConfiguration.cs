using Management.Photo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Management.Photo.Infrastructure.Persistence.EntitiesConfigure
{
    public class BucketConfiguration : IEntityTypeConfiguration<BucketEntity>
    {
        public void Configure(EntityTypeBuilder<BucketEntity> modelBuilder)
        {
            _ = modelBuilder.Property(x => x.BucketName).IsUnicode(false).HasMaxLength(255).IsRequired();

            _ = modelBuilder.Property(x => x.BucketDescription).HasMaxLength(255);

            _ = modelBuilder.HasMany(x => x.StoreInfos)
                .WithOne(x => x.Bucket)
                .HasForeignKey(x => x.BucketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
