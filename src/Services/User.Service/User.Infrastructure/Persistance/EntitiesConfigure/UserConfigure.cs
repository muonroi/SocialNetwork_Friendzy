namespace User.Infrastructure.Persistance.EntitiesConfigure;

public class UserConfigure : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> modelBuilder)
    {
        _ = modelBuilder
            .HasIndex(x => new { x.FirstName, x.LastName })
            .HasDatabaseName("IX_Names_Ascending");

        _ = modelBuilder
           .HasIndex(x => new { x.FirstName, x.LastName })
           .HasDatabaseName("IX_Names_Descending");
    }
}