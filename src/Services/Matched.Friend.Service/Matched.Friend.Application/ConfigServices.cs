using Infrastructure.Extensions;
using Matched.Friend.Application.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Matched.Friend.Application
{
    public static class ConfigServices
    {
        public static IServiceCollection AddConfigurationApplication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            Assembly assemblyInstance = Assembly.GetExecutingAssembly();
            IHttpContextAccessor httpContextAccessor = services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();
            _ = services.AddApplicationServices(assemblyInstance);
            _ = services.AddConfigurationSettings(configuration, environment, httpContextAccessor);
            return services;
        }
    }
}
