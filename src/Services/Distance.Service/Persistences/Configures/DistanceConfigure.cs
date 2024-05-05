using Distance.Service.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Distance.Service.Persistences.Configures;

public class DistanceConfigure : IEntityTypeConfiguration<DistanceEntity>
{
    public void Configure(EntityTypeBuilder<DistanceEntity> modelBuilder)
    {
    }
}