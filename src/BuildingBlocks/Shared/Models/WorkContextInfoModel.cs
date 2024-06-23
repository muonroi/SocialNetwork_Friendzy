namespace Shared.Models;

public record WorkContextInfoModel

{
    public string CorrelationId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Fullname => $"{FirstName} {LastName}";
    public string Caller { get; set; } = string.Empty;
    public string ClientIpAddr { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Roles { get; set; } = string.Empty;
    public string AgentCode { get; set; } = string.Empty;
    public long UserId { get; set; }
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
    public bool IsAuthenticated { get; set; }
    public string AccountId { get; set; } = string.Empty;
}