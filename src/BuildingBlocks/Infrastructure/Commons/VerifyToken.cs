namespace Infrastructure.Commons;

public sealed class VerifyToken(bool isAuthenticated)
{
    public int UserId { get; set; }

    public bool IsAuthenticated { get; set; } = isAuthenticated;

    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string AgentCode { get; set; } = string.Empty;

    public string LinerCode { get; set; } = string.Empty;

    public bool IsMasterAccount { get; set; }

    public List<int>? RelatedAccounts { get; set; }
}