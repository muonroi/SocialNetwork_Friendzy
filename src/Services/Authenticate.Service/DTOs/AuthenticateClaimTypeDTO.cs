namespace Authenticate.Service.DTOs;

public record AuthenticateClaimTypeDTO
{
    public required string PhoneNumber { get; init; }
    public int RoleId { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public long UserId { get; init; }
    public required string EmailAddress { get; init; }
    public bool IsActive { get; init; }
    public double Balance { get; init; }
    public bool IsEmailVerify { get; init; }
    public int AccountStatus { get; init; }
    public int Currency { get; init; }
    public int AccountType { get; init; }
}