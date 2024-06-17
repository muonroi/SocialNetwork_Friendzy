namespace Contracts.DTOs.JwtBearerDTOs;

public class JwtBearerConfig
{
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}