using Shared.DTOs;

namespace Infrastructure.Middleware;

public class WorkContextMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        IWorkContextAccessor accessor = context.RequestServices.GetRequiredService<IWorkContextAccessor>();

        IHeaderDictionary headers = context.Request.Headers;
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.CorrelationId), out StringValues correlationIds);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.Username), out StringValues usernames);
        _ = headers.TryGetValue("Accept-Language", out StringValues languages);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.Roles), out StringValues roles);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.AgentCode), out StringValues agentCodes);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.LinerCode), out StringValues linerCodes);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.UserId), out StringValues userIds);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.IsMasterAccount), out StringValues isMasterAccounts);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.RelatedAccounts), out StringValues relatedAccountsTmp);

        string clientIpAddr = context.GetRequestedIpAddress();
        string caller = context.GetHeaderUserAgent();
        _ = int.TryParse(userIds.FirstOrDefault(), out int userId);
        _ = bool.TryParse(isMasterAccounts.FirstOrDefault(), out bool IsMasterAccount);

        List<int>? relatedAccounts = relatedAccountsTmp
            .FirstOrDefault()?
            .Split(",").Where(x => !string.IsNullOrEmpty(x))
            .Select(x => Convert.ToInt32(x))
            .ToList();

        accessor.WorkContext = new()
        {
            CorrelationId = correlationIds.FirstOrDefault() ?? Guid.NewGuid().ToString(),
            ClientIpAddr = clientIpAddr,
            Caller = caller,
            Language = languages.FirstOrDefault() ?? "vi-VN",
            Username = usernames.FirstOrDefault() ?? string.Empty,
            Roles = roles.FirstOrDefault() ?? string.Empty,
            AgentCode = agentCodes.FirstOrDefault() ?? string.Empty,
            LinerCode = linerCodes.FirstOrDefault() ?? string.Empty,
            IsMasterAccount = IsMasterAccount,
            UserId = userId,
            RelatedAccounts = relatedAccounts
        };
        await next(context);
    }
}