namespace Message.Domain.Entities
{
    public class MessageEntity(
        AuthorEntity author,
        long createdAt,
        string id,
        string status,
        string text,
        string type,
        int height,
        string name,
        int size, string uri,
        int width,
        string mimeType,
        TimeSpan? duration = null)
    {
        public AuthorEntity Author { get; set; } = author;
        public long CreatedAt { get; set; } = createdAt;
        public string Id { get; set; } = id;
        public string Status { get; set; } = status;
        public string Text { get; set; } = text;
        public string Type { get; set; } = type;
        public int Height { get; set; } = height;
        public string Name { get; set; } = name;
        public int Size { get; set; } = size;
        public string Uri { get; set; } = uri;
        public int Width { get; set; } = width;
        public string MimeType { get; set; } = mimeType;
        public TimeSpan? Duration { get; set; } = duration;
    }
}
