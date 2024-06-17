namespace Management.Friends.Action.Infrastructure.Persistence.EntitiesConfigure;

public class FriendsActionConfigure : IEntityTypeConfiguration<FriendsActionEntity>
{
    public void Configure(EntityTypeBuilder<FriendsActionEntity> modelBuilder)
    {
        _ = modelBuilder
            .HasKey(x => new { x.FriendId, x.UserId });
    }
}