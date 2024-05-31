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
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.UserId), out StringValues userIds);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.FirstName), out StringValues firstNames);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.LastName), out StringValues lastNames);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.PhoneNumber), out StringValues phoneNumbers);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.RoleIds), out StringValues roleIds);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.EmailAddress), out StringValues emailAddresses);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.IsActive), out StringValues isActives);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.Balance), out StringValues balances);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.IsEmailVerify), out StringValues isEmailVerifies);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.AccountStatus), out StringValues accountStatuses);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.Currency), out StringValues currencies);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.AccountType), out StringValues accountTypes);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.IsAuthenticated), out StringValues isAuthenticateds);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.Latitude), out StringValues latitudes);
        _ = headers.TryGetValue(nameof(WorkContextInfoDTO.Longitude), out StringValues longitudes);

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

        accessor.WorkContext = new WorkContextInfoDTO
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
            Longitude = longitude
        };

        await next(context);
    }

}