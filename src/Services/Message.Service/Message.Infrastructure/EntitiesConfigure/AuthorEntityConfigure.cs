namespace Message.Infrastructure.EntitiesConfigure;

public class AuthorEntityConfigure : IEntityTypeConfiguration<AuthorEntity>
{
    public void Configure(EntityTypeBuilder<AuthorEntity> modelBuilder)
    {
        modelBuilder.ToCollection("authors");
    }
}