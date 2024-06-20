namespace Message.Application.Infrastructure.Dtos
{
    public class CreateMessageDto
    {
        public string RecipientAccountId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
