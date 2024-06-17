using Message.Domain.Entities;

public class LastMessageChat
{
    public string SenderId { get; set; } = string.Empty;
    public string SenderUsername { get; set; } = string.Empty;
    public Author? Sender { get; set; }
    public string RecipientId { get; set; } = string.Empty;
    public string RecipientUsername { get; set; } = string.Empty;
    public Author? Recipient { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime MessageLastDate { get; set; } = DateTime.UtcNow;
    public string GroupName { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
}