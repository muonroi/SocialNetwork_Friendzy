namespace Infrastructure.Commons;
public sealed class VerifyToken(bool isAuthenticated)
{
    public int UserId { get; set; }

    public bool IsAuthenticated { get; set; } = isAuthenticated;

    public string FullName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string RoleIds { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public string Balance { get; set; } = string.Empty;

    public bool IsEmailVerify { get; set; }

    public int AccountStatus { get; set; }

    public int Currency { get; set; }

    public int AccountType { get; set; }
}
