namespace ExternalAPI.Models;
public record PushNotificationMessageTextHub
{
    public required string MessageText { get; set; }
    public required string SenderAccountId { get; set; }
    public required string RecipientAccountId { get; set; }
}