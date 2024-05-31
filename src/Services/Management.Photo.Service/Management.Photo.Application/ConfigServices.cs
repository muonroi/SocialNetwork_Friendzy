using Infrastructure.Extensions;
using Management.Photo.Application.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Management.Photo.Application
{
    public static class ConfigServices
    {
        public static IServiceCollection AddConfigurationApplication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            Assembly assemblyInstance = Assembly.GetExecutingAssembly();
            _ = services.AddApplicationServices(assemblyInstance);
            _ = services.AddConfigurationSettings(configuration, environment);
            return services;
        }
    }
}
