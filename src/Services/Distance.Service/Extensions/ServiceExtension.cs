using Contracts.Commons.Constants;

namespace Distance.Service.Extensions;

internal static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = services.AddDbContext<DistanceDbContext>(options =>
        {
            _ = options.UseSqlServer(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString),
                builder => builder.MigrationsAssembly(typeof(DistanceDbContext).Assembly.FullName));
        });
        _ = services.AddScoped<DistanceDbContextSeed>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped<IDistanceServiceRepository, DistanceServiceRepository>();
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        _ = ServiceExtensionCommon.AddConfigurationSettingsCommon(services, configuration);
        _ = services.AddDapperForMSSQL();
        services.AddGrpcServer();
        return services;
    }
}