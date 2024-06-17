namespace ExternalAPI.Models;

public record AccountDataModel
{
    public Guid AccountId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}