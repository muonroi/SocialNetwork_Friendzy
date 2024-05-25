using Contracts.Commons.Constants;
using Distance.Service.Persistance;

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
        _ = services.AddConfigurationSettingsThirdExtenal(configuration);
        _ = services.AddDapperForMSSQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        services.AddGrpcServer();
        return services;
    }
}