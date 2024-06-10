using Infrastructure.Helper;

namespace SearchPartners.Aggregate.Service.Extensions;

public static class AuthenticationExtention
{
    private static IApplicationBuilder UseJwtMiddleware(
        this IApplicationBuilder app,
        Func<IServiceProvider, HttpContext, Task<VerifyToken>> callback)
    {
        return app.UseMiddleware<JwtMiddleware>(callback);
    }

    public static IApplicationBuilder UseAuthenticationMiddleware(
        this IApplicationBuilder app, IConfiguration configuration)
    {
        _ = app.UseJwtMiddleware(async (serviceProvider, context) =>
        {
            IHeaderDictionary headers = context.Request.Headers;

            string? accessToken = headers.Authorization.FirstOrDefault(header => header?.StartsWith("Bearer ") == true)?["Bearer ".Length..];

            if (string.IsNullOrEmpty(accessToken))
            {
                return new(false);
            }

            GrpcClientFactory grpcClientFactory = serviceProvider.GetRequiredService<GrpcClientFactory>();

            AuthenticateVerifyClient authenticateClient = grpcClientFactory.CreateClient<AuthenticateVerifyClient>(ServiceConstants.AuthenticateService);

            VerifyTokenReply validateToken = await authenticateClient.VerifyTokenAsync(new VerifyTokenRequest
            {
                AccessToken = accessToken,
                Issuer = configuration[ConfigurationSetting.JwtIssuer],
                Audience = configuration[ConfigurationSetting.JwtAudience],
                SecretKey = configuration.GetConfigHelper(ConfigurationSetting.JwtSecrectKey),
            });

            return !validateToken.IsAuthenticated
                ? new(validateToken.IsAuthenticated)
                : new(validateToken.IsAuthenticated)
                {
                    UserId = (int)validateToken.UserId,
                    FullName = validateToken.FullName,
                    Role = validateToken.RoleIds,
                    Latitude = validateToken.Latitude,
                    Longitude = validateToken.Longitude,
                    PhoneNumber = validateToken.PhoneNumber,
                    RoleIds = validateToken.RoleIds,
                    EmailAddress = validateToken.EmailAddress,
                    IsActive = validateToken.IsActive,
                    Balance = validateToken.Balance,
                    IsEmailVerify = validateToken.IsEmailVerify,
                    AccountStatus = validateToken.AccountStatus,
                    Currency = validateToken.Currency,
                    AccountType = validateToken.AccountType
                };
        });

        return app;
    }
}