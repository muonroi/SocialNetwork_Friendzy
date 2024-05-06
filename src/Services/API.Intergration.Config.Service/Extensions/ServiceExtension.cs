﻿namespace API.Intergration.Config.Service.Extensions
{
    internal static class ServiceExtension
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
        {
            _ = services.AddScoped<IDapperCustom, DapperCustom>();
            _ = services.AddDapperForPostgreSQL();
            _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
            _ = services.AddDapperCaching(configuration);
            services.AddGrpcServer();
            return services;
        }
    }
}