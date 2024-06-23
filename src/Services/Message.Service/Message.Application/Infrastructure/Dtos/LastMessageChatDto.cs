namespace Message.Application.Infrastructure.Dtos
{
    public class LastMessageChatDto
    {
        public string Id { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string SenderAccountGuid { get; set; } = string.Empty;
        public string SenderDisplayName { get; set; } = string.Empty;
        public string SenderImgUrl { get; set; } = string.Empty;
        public string RecipientId { get; set; } = string.Empty;
        public string RecipientAccountGuid { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime MessageLastDate { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public UserDataModel Sender { get; set; } = new UserDataModel();
        public UserDataModel Recipient { get; set; } = new UserDataModel();
        public Author? QuickSenderInfo { get; set; }
        public Author? QuickRecipientInfo { get; set; }
    }
}