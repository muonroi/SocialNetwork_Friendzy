namespace Message.Domain.Entities;

public class Author
{
    public required string Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DisplayName => $"{FirstName} {LastName}";
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime? LastActive { get; set; }
    public ICollection<MessageEntry> MessagesSent { get; set; } = [];
    public ICollection<MessageEntry> MessagesReceived { get; set; } = [];
    public ICollection<LastMessageChat> LastMessageChatsSent { get; set; } = [];
    public ICollection<LastMessageChat> LastMessageChatsReceived { get; set; } = [];
}