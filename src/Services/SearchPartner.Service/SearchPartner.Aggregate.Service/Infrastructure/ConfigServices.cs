namespace SearchPartners.Aggregate.Service.Infrastructure;

public static class ConfigServices
{
    public static IServiceCollection AddConfigurationApplication(this IServiceCollection services)
    {
        Assembly assemblyInstance = Assembly.GetExecutingAssembly();
        _ = services.AddApplicationServices(assemblyInstance);
        return services;
    }
}
