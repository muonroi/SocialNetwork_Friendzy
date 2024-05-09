namespace API.Intergration.Config.Service.Extensions
{
    internal static class ServiceExtension
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
        {
            _ = ServiceExtensionCommon.AddConfigurationSettingsCommon(services, configuration);
            _ = services.AddDapperForPostgreSQL();
            services.AddGrpcServer();
            return services;
        }
    }
}