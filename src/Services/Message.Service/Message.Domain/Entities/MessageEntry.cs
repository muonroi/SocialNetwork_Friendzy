namespace Message.Domain.Entities;

[BsonCollectionAttribute("Messages")]
public class MessageEntry : MongoDbEntity
{
    public string? SenderId { get; set; }
    public string SenderAccountId { get; set; } = string.Empty;
    public Author? Sender { get; set; }
    public string? RecipientId { get; set; }
    public string RecipientAccountId { get; set; } = string.Empty;
    public Author? Recipient { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
}