namespace ExternalAPI.Models;
public record PushNotificationMessageTextHub
{
    public string AccountId { get; set; } = string.Empty;
    public string MessageText { get; set; } = string.Empty;
}
