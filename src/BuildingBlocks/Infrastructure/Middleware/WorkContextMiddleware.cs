using Shared.Models;

namespace Infrastructure.Middleware;

public class WorkContextMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        IWorkContextAccessor accessor = context.RequestServices.GetRequiredService<IWorkContextAccessor>();

        IHeaderDictionary headers = context.Request.Headers;
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.CorrelationId), out StringValues correlationIds);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.Username), out StringValues usernames);
        _ = headers.TryGetValue("Accept-Language", out StringValues languages);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.Roles), out StringValues roles);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.AgentCode), out StringValues agentCodes);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.UserId), out StringValues userIds);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.FirstName), out StringValues firstNames);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.LastName), out StringValues lastNames);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.PhoneNumber), out StringValues phoneNumbers);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.RoleIds), out StringValues roleIds);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.EmailAddress), out StringValues emailAddresses);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.IsActive), out StringValues isActives);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.Balance), out StringValues balances);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.IsEmailVerify), out StringValues isEmailVerifies);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.AccountStatus), out StringValues accountStatuses);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.Currency), out StringValues currencies);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.AccountType), out StringValues accountTypes);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.IsAuthenticated), out StringValues isAuthenticateds);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.Latitude), out StringValues latitudes);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.Longitude), out StringValues longitudes);
        _ = headers.TryGetValue(nameof(WorkContextInfoModel.AccountId), out StringValues accountId);

        string clientIpAddr = context.GetRequestedIpAddress();
        string caller = context.GetHeaderUserAgent();
        _ = int.TryParse(userIds.FirstOrDefault(), out int userId);
        _ = double.TryParse(latitudes.FirstOrDefault(), out double latitude);
        _ = double.TryParse(longitudes.FirstOrDefault(), out double longitude);
        _ = bool.TryParse(isActives.FirstOrDefault(), out bool isActive);
        _ = bool.TryParse(isEmailVerifies.FirstOrDefault(), out bool isEmailVerify);
        _ = bool.TryParse(isAuthenticateds.FirstOrDefault(), out bool isAuthenticated);
        _ = int.TryParse(accountStatuses.FirstOrDefault(), out int accountStatus);
        _ = int.TryParse(currencies.FirstOrDefault(), out int currency);
        _ = int.TryParse(accountTypes.FirstOrDefault(), out int accountType);

        accessor.WorkContext = new WorkContextInfoModel
        {
            CorrelationId = correlationIds.FirstOrDefault() ?? Guid.NewGuid().ToString(),
            ClientIpAddr = clientIpAddr,
            Caller = caller,
            Language = languages.FirstOrDefault() ?? "vi-VN",
            Username = usernames.FirstOrDefault() ?? string.Empty,
            Roles = roles.FirstOrDefault() ?? string.Empty,
            AgentCode = agentCodes.FirstOrDefault() ?? string.Empty,
            UserId = userId,
            FirstName = firstNames.FirstOrDefault() ?? string.Empty,
            LastName = lastNames.FirstOrDefault() ?? string.Empty,
            PhoneNumber = phoneNumbers.FirstOrDefault() ?? string.Empty,
            RoleIds = roleIds.FirstOrDefault() ?? string.Empty,
            EmailAddress = emailAddresses.FirstOrDefault() ?? string.Empty,
            IsActive = isActive,
            Balance = balances.FirstOrDefault() ?? string.Empty,
            IsEmailVerify = isEmailVerify,
            AccountStatus = accountStatus,
            Currency = currency,
            AccountType = accountType,
            IsAuthenticated = isAuthenticated,
            Latitude = latitude,
            Longitude = longitude,
            AccountId = accountId.FirstOrDefault() ?? string.Empty,
        };

        await next(context);
    }
}