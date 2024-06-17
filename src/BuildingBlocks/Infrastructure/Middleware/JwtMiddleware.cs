namespace Infrastructure.Middleware;

public class JwtMiddleware(
    RequestDelegate next,
    Func<IServiceProvider, HttpContext, Task<VerifyToken>> callbackVerifyToken)
{
    private readonly RequestDelegate _next = next;
    private readonly Func<IServiceProvider, HttpContext, Task<VerifyToken>> _callbackVerifyToken = callbackVerifyToken;

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        CancellationToken cancellationToken = context.RequestAborted;

        IHeaderDictionary headers = context.Request.Headers;
        if (IsAllowAnonymous(context))
        {
            await _next(context);
            return;
        }
        _ = headers.TryGetValue("Accept-Language", out StringValues language);

        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.CorrelationId), out StringValues correlationIds);

        AddHeader(headers, nameof(WorkContextInfoDTO.Language), language.FirstOrDefault() ?? "vi-VN");

        if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
        {
            string token = value.ToString();
            AddHeader(headers, "Authorization", $"Bearer {token}");
        }

        VerifyToken verifyToken = await _callbackVerifyToken(serviceProvider, context);

        if (!verifyToken.IsAuthenticated)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }
        AddHeader(headers, nameof(WorkContextInfoDTO.UserId), long.Parse(verifyToken.UserId.ToString()).ToString());
        AddHeader(headers, nameof(WorkContextInfoDTO.Fullname), verifyToken.FullName);
        AddHeader(headers, nameof(WorkContextInfoDTO.Roles), verifyToken.Role);
        AddHeader(headers, nameof(WorkContextInfoDTO.Latitude), verifyToken.Latitude.ToString());
        AddHeader(headers, nameof(WorkContextInfoDTO.Longitude), verifyToken.Longitude.ToString());
        AddHeader(headers, nameof(WorkContextInfoDTO.PhoneNumber), verifyToken.PhoneNumber);
        AddHeader(headers, nameof(WorkContextInfoDTO.RoleIds), verifyToken.RoleIds);
        AddHeader(headers, nameof(WorkContextInfoDTO.EmailAddress), verifyToken.EmailAddress);
        AddHeader(headers, nameof(WorkContextInfoDTO.IsActive), verifyToken.IsActive.ToString());
        AddHeader(headers, nameof(WorkContextInfoDTO.Balance), verifyToken.Balance);
        AddHeader(headers, nameof(WorkContextInfoDTO.IsEmailVerify), verifyToken.IsEmailVerify.ToString());
        AddHeader(headers, nameof(WorkContextInfoDTO.AccountStatus), verifyToken.AccountStatus.ToString());
        AddHeader(headers, nameof(WorkContextInfoDTO.Currency), verifyToken.Currency.ToString());
        AddHeader(headers, nameof(WorkContextInfoDTO.AccountType), verifyToken.AccountType.ToString());
        AddHeader(headers, nameof(WorkContextInfoDTO.IsAuthenticated), verifyToken.IsAuthenticated.ToString());
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