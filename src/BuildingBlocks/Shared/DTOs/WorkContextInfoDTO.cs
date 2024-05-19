namespace Shared.DTOs;

public record WorkContextInfoDTO

{
    public string CorrelationId { get; set; } = string.Empty;
    public string Caller { get; set; } = string.Empty;
    public string ClientIpAddr { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Roles { get; set; } = string.Empty;
    public string AgentCode { get; set; } = string.Empty;
    public string LinerCode { get; set; } = string.Empty;
    public long UserId { get; set; }
    public bool IsMasterAccount { get; set; }
    public List<int>? RelatedAccounts { get; set; }

    public List<int> AccountIds
    {
        get
        {
            List<int> ids = [];
            if (IsMasterAccount && RelatedAccounts is not null && RelatedAccounts.Count != 0)
            {
                ids.AddRange(RelatedAccounts);
            }
            if (UserId > 0)
            {
                ids.Add((int)UserId);
            }
            return ids;
        }
    }
}