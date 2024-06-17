namespace Message.Infrastructure.Dtos
{
    public record MessageResponse : MessageDto
    {
        public string? Id { get; set; }
    }
}
