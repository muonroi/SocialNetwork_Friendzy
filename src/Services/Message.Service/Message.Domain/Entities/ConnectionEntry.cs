namespace Message.Domain.Entities;

[BsonCollection("Connections")]
public class ConnectionEntry : MongoDbEntity
{
    public ConnectionEntry()
    { }

    public ConnectionEntry(string connectionId, string accountId)
    {
        ConnectionId = connectionId;
        AccountId = accountId;
    }

    public string ConnectionId { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
}