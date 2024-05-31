using Infrastructure.Extensions;
namespace Authenticate.Service.Extensions;

internal static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services)
    {
        services.AddGrpcServer();
        return services;
    }
}