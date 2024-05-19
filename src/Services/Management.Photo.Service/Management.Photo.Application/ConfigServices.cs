﻿using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Management.Photo.Application
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
