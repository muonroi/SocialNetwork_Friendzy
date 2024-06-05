

namespace ExternalAPI.DTOs;

public record CreateAccountDto
{
    public AccountType AccountType { get; set; }

    public Currency Currency { get; set; }

    public string LockReason { get; set; } = string.Empty;

    public virtual decimal Balance { get; set; }

    public bool IsActive { get; set; }

    public bool IsEmailVerified { get; set; }

    public AccountStatus Status { get; set; }

    public string Roles { get; set; } = string.Empty;
    public long UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}
