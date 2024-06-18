namespace Message.Application.Infrastructure.Dtos;

public record MessageResponse : MessageDto
{
    public string? Id { get; set; }
}