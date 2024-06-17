namespace Message.Infrastructure.EntitiesConfigure;

public class MessageEntityConfigure : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> modelBuilder)
    {
        modelBuilder.ToCollection("messages");
    }
}