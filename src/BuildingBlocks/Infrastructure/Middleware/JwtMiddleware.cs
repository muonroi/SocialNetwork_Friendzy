using Shared.DTOs;

namespace Infrastructure.Middleware;

public class JwtMiddleware(
    RequestDelegate next,
    Func<IServiceProvider, HttpContext, Task<VerifyToken>> callbackVerifyToken,
    IConfiguration configuration)
{
    private readonly RequestDelegate _next = next;

    private readonly IConfiguration _configuration = configuration;

    private readonly Func<IServiceProvider, HttpContext, Task<VerifyToken>> _callbackVerifyToken = callbackVerifyToken;

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        CancellationToken cancellationToken = context.RequestAborted;

        IHeaderDictionary headers = context.Request.Headers;

        if (IsAllowAnonymous(context))
        {
            AddHeader(headers, nameof(WorkContextInfoDTO.CorrelationId), Guid.NewGuid().ToString());

            await _next(context);
            return;
        }

        IWorkContextAccessor workContextAccessor = context.RequestServices.GetRequiredService<IWorkContextAccessor>();

        VerifyToken verifyToken = await _callbackVerifyToken(serviceProvider, context);

        if (!verifyToken.IsAuthenticated)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        _ = headers.TryGetValue("Accept-Language", out StringValues language);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.CorrelationId), out StringValues correlationIds);

        AddHeader(headers, nameof(WorkContextInfoDTO.Username), verifyToken.Username);
        AddHeader(headers, nameof(WorkContextInfoDTO.UserId), verifyToken.UserId.ToString());
        AddHeader(headers, nameof(WorkContextInfoDTO.Roles), verifyToken.Role);
        AddHeader(headers, nameof(WorkContextInfoDTO.IsMasterAccount), verifyToken.IsMasterAccount.ToString());
        AddHeader(headers, nameof(WorkContextInfoDTO.RelatedAccounts), string.Join(",", verifyToken.RelatedAccounts ?? []));
        AddHeader(headers, "Accept-Language", language.FirstOrDefault() ?? "vi-VN");

        if (!string.IsNullOrEmpty(verifyToken.AgentCode))
        {
            AddHeader(headers, "AgentCode", verifyToken.AgentCode);
        }
        if (!string.IsNullOrEmpty(verifyToken.LinerCode))
        {
            AddHeader(headers, "LinerCode", verifyToken.LinerCode);
        }

        if (correlationIds.Count == 0)
        {
            AddHeader(headers, nameof(WorkContextInfoDTO.CorrelationId), Guid.NewGuid().ToString());
        }
        Claim username = new(nameof(WorkContextInfoDTO.Username), verifyToken.Username);
        context.User = new(new ClaimsIdentity([username], JwtBearerDefaults.AuthenticationScheme));
        await _next(context);
    }

    private static bool IsAllowAnonymous(HttpContext httpContext)
    {
        Endpoint? endpoint = httpContext.GetEndpoint();
        if (endpoint is null)
        {
            return false;
        }

        AllowAnonymousAttribute? endpointMetadata = endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>();
        AuthorizeAttribute? authorize = endpoint.Metadata.GetMetadata<AuthorizeAttribute>();

        return (endpointMetadata is null && authorize is null) || endpointMetadata is not null;
    }

    private static void AddHeader(IHeaderDictionary headers, string key, string value)
    {
        if (headers.ContainsKey(key))
        {
            _ = headers.Remove(key);
        }
        headers.Append(key, value);
    }
}