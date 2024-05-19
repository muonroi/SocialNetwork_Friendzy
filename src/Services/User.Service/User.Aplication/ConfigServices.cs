using Infrastructure.Extensions;

namespace User.Application
{
    public static class ConfigServices
    {
        public static IServiceCollection AddConfigurationApplication(this IServiceCollection services)
        {
            Assembly assemblyInstance = Assembly.GetExecutingAssembly();
            _ = services.AddApplicationServices(assemblyInstance);
            return services;
        }
    }
}
