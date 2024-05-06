namespace User.API.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        SmtpConfig? emailSettings = configuration.GetSection(nameof(SmtpConfig)).Get<SmtpConfig>();
        if (emailSettings is null)
        {
            return services;
        }
        _ = services.AddSingleton(emailSettings);
        _ = services.AddScoped<IDapperCustom, DapperCustom>();
        _ = services.AddDapperForMSSQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        return services;
    }
}