namespace Message.Application.Infrastructure.Dtos
{
    public class LastMessageChatDto
    {
        public Guid Id { get; set; }
        public required string SenderId { get; set; }
        public required string SenderAccountGuid { get; set; }
        public required string SenderDisplayName { get; set; }
        public required string SenderImgUrl { get; set; }
        public required string RecipientId { get; set; }
        public required string RecipientAccountGuid { get; set; }
        public required string Content { get; set; }
        public DateTime MessageLastDate { get; set; }
        public required string GroupName { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
