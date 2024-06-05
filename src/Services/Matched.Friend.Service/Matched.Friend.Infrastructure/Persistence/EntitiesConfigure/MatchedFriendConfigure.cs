namespace Matched.Friend.Infrastructure.Persistence.EntitiesConfigure;

public class FriendsMatchedConfigure : IEntityTypeConfiguration<FriendsMatchedEntity>
{
    public void Configure(EntityTypeBuilder<FriendsMatchedEntity> modelBuilder)
    {
        _ = modelBuilder
            .HasKey(x => new { x.FriendId, x.UserId });
    }
}