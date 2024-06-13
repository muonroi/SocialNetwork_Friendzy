namespace SearchPartners.Service.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = configuration.ToBase64();
        services.AddGrpcServer();
        return services;
    }
}