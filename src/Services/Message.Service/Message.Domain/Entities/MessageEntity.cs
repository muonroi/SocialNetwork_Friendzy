namespace Message.Domain.Entities;

public class MessageEntity : EntityAuditBase<string>
{
    public AuthorEntity? Author { get; set; }
    public string? AuthorId { get; set; }
    public long CreatedAt { get; set; }
    public string? Status { get; set; }
    public string? Text { get; set; }
    public string? Type { get; set; }
    public int Height { get; set; }
    public string? Name { get; set; }
    public int Size { get; set; }
    public string? Uri { get; set; }
    public int Width { get; set; }
    public string? MimeType { get; set; }
    public TimeSpan? Duration { get; set; }

    public MessageEntity()
    {
    }

    public MessageEntity(

        AuthorEntity author,
        string authorId,
        long createdAt,
        string status,
        string text,
        string type,
        int height,
        string name,
        int size,
        string uri,
        int width,
        string mimeType,
        TimeSpan? duration = null)
    {
        AuthorId = authorId;
        Author = author;
        CreatedAt = createdAt;
        Status = status;
        Text = text;
        Type = type;
        Height = height;
        Name = name;
        Size = size;
        Uri = uri;
        Width = width;
        MimeType = mimeType;
        Duration = duration;
    }
}