using Contracts.Commons.Constants;
using Dapper.Extensions;
using Infrastructure.ORMs.Dapper;

namespace Setting.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<SettingDbContext>(options =>
        {
            _ = options.UseSqlServer(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString),
                builder => builder.MigrationsAssembly(typeof(SettingDbContext).Assembly.FullName));
        });
        _ = services.AddScoped<SettingDbContextSeed>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped(typeof(ISettingRepository<,>), typeof(SettingRepository<,>));
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        _ = services.AddConfigurationSettingsThirdExtenal(configuration);
        _ = services.AddDapperForMSSQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);

        return services;
    }
}