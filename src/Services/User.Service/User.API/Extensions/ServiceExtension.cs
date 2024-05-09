namespace User.Service.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = ServiceExtensionCommon.AddConfigurationSettingsCommon(services, configuration);
        _ = services.AddDapperForMSSQL();
        return services;
    }
}