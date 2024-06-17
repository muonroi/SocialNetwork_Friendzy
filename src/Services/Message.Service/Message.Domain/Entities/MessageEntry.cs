using Shared.Attributes;

namespace Message.Domain.Entities;

[BsonCollectionAttribute("Messages")]
public class MessageEntry : MongoDbEntity
{
    public string? SenderId { get; set; }
    public required string SenderAccountId { get; set; }
    public required Author Sender { get; set; }
    public string? RecipientId { get; set; }
    public required string RecipientAccountId { get; set; }
    public required Author Recipient { get; set; }
    public required string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
}
