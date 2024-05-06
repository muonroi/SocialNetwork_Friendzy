namespace Post.Infrastructure.Persistence.EntitiesConfigure;

public class PostConfigure : IEntityTypeConfiguration<PostEnitity>
{
    public void Configure(EntityTypeBuilder<PostEnitity> modelBuilder)
    {
        _ = modelBuilder.HasKey(x => x.Id);

        _ = modelBuilder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        _ = modelBuilder.Property(x => x.Content)
            .IsRequired();

        _ = modelBuilder.Property(x => x.ImageUrl)
            .HasMaxLength(500);

        _ = modelBuilder.Property(x => x.VideoUrl)
            .HasMaxLength(500);

        _ = modelBuilder.Property(x => x.AudioUrl)
            .HasMaxLength(500);

        _ = modelBuilder.Property(x => x.FileUrl)
            .HasMaxLength(500);

        _ = modelBuilder.Property(x => x.Slug)
            .HasMaxLength(200);

        _ = modelBuilder.Property(x => x.IsPublished)
            .IsRequired();

        _ = modelBuilder.Property(x => x.IsDeleted)
            .IsRequired();

        _ = modelBuilder.Property(x => x.CategoryId)
            .IsRequired();

        _ = modelBuilder.Property(x => x.AuthorId)
            .IsRequired();
    }
}