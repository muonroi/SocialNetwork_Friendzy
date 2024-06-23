using Shared.Models;

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

        _ = headers.TryGetValue(nameof(WorkContextInfoModel.CorrelationId), out StringValues correlationIds);

        AddHeader(headers, nameof(WorkContextInfoModel.Language), language.FirstOrDefault() ?? "vi-VN");

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
        AddHeader(headers, nameof(WorkContextInfoModel.UserId), long.Parse(verifyToken.UserId.ToString()).ToString());
        AddHeader(headers, nameof(WorkContextInfoModel.Fullname), verifyToken.FullName);
        AddHeader(headers, nameof(WorkContextInfoModel.Roles), verifyToken.Role);
        AddHeader(headers, nameof(WorkContextInfoModel.Latitude), verifyToken.Latitude.ToString());
        AddHeader(headers, nameof(WorkContextInfoModel.Longitude), verifyToken.Longitude.ToString());
        AddHeader(headers, nameof(WorkContextInfoModel.PhoneNumber), verifyToken.PhoneNumber);
        AddHeader(headers, nameof(WorkContextInfoModel.RoleIds), verifyToken.RoleIds);
        AddHeader(headers, nameof(WorkContextInfoModel.EmailAddress), verifyToken.EmailAddress);
        AddHeader(headers, nameof(WorkContextInfoModel.IsActive), verifyToken.IsActive.ToString());
        AddHeader(headers, nameof(WorkContextInfoModel.Balance), verifyToken.Balance);
        AddHeader(headers, nameof(WorkContextInfoModel.IsEmailVerify), verifyToken.IsEmailVerify.ToString());
        AddHeader(headers, nameof(WorkContextInfoModel.AccountStatus), verifyToken.AccountStatus.ToString());
        AddHeader(headers, nameof(WorkContextInfoModel.Currency), verifyToken.Currency.ToString());
        AddHeader(headers, nameof(WorkContextInfoModel.AccountType), verifyToken.AccountType.ToString());
        AddHeader(headers, nameof(WorkContextInfoModel.IsAuthenticated), verifyToken.IsAuthenticated.ToString());
        AddHeader(headers, nameof(WorkContextInfoModel.AccountId), verifyToken.AccountId);
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