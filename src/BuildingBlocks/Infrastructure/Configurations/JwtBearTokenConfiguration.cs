using Contracts.Commons.Constants;
using Infrastructure.Helper;

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
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration[ConfigurationSetting.JwtIssuer],
                ValidAudience = configuration[ConfigurationSetting.JwtAudience],
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(configuration.GetConfigHelper(ConfigurationSetting.JwtSecrectKey)))
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
                }
            };
        });

        _ = services.AddAuthorization();
        return services;
    }
}