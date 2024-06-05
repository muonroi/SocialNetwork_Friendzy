namespace Contracts.DTOs.JwtBearerDTOs;

public class JwtBearerConfig
{
    public required string Audience { get; set; }
    public required string Issuer { get; set; }
    public required string Key { get; set; }
}
