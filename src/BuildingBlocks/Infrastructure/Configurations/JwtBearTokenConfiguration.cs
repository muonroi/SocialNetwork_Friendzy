namespace Infrastructure.Configurations;

public static class JwtBearTokenConfiguration
{
    public static IServiceCollection ConfigureJwtBearerToken(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                ValidIssuer = configuration[ConfigurationSetting.JwtIssuer],
                ValidAudience = configuration[ConfigurationSetting.JwtAudience],
                IssuerSigningKeys =
                [
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetConfigHelper(ConfigurationSetting.JwtSecrectKey)))
                    {
                        KeyId = configuration.GetConfigHelper(ConfigurationSetting.JwtSecrectKey)
                    }
                ],
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    if (context.Principal?.Identity is ClaimsIdentity userClaims)
                    {
                        string? roleClaims = context.Principal.FindFirst("roles")?.Value;
                        if (roleClaims != null)
                        {
                            string[] roles = roleClaims.Split(',');
                            foreach (string role in roles)
                            {
                                userClaims.AddClaim(new Claim(ClaimTypes.Role, role));
                            }
                        }
                    }
                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    ILogger<JwtBearerEvents> logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    StringValues accessToken = context.Request.Query["access_token"];
                    PathString path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    ILogger<JwtBearerEvents> logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    logger.LogError("Authentication failed: {Exception}", context.Exception);
                    return Task.CompletedTask;
                }
            };
        });

        _ = services.AddAuthorization();
        return services;
    }
}