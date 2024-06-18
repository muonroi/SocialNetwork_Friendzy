namespace Message.Domain.Models;

public class Author
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DisplayName => $"{FirstName} {LastName}";
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime? LastActive { get; set; }
    public ICollection<MessageEntry> MessagesSent { get; set; } = [];
    public ICollection<MessageEntry> MessagesReceived { get; set; } = [];
    public ICollection<LastMessageChatEntry> LastMessageChatsSent { get; set; } = [];
    public ICollection<LastMessageChatEntry> LastMessageChatsReceived { get; set; } = [];
}