[BsonCollectionAttribute("LastMessageChats")]
public class LastMessageChatEntry : MongoDbEntity
{
    public string SenderId { get; set; } = string.Empty;
    public string SenderAccountId { get; set; } = string.Empty;
    public Author? Sender { get; set; }
    public string RecipientId { get; set; } = string.Empty;
    public string RecipienAccountId { get; set; } = string.Empty;
    public Author? Recipient { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime MessageLastDate { get; set; } = DateTime.UtcNow;
    public string GroupName { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
}