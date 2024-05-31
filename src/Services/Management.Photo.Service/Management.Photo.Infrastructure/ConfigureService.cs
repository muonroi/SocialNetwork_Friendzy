using Contracts.Commons.Constants;
using Contracts.Commons.Interfaces;
using Contracts.Services;
using Dapper.Extensions;
using Dapper.Extensions.MSSQL;
using Infrastructure.Commons;
using Infrastructure.Extensions;
using Infrastructure.Helper;
using Infrastructure.ORMs.Dapper;
using Infrastructure.Services;
using Management.Photo.Application.Commons.Interfaces;
using Management.Photo.Infrastructure.Persistence;
using Management.Photo.Infrastructure.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Management.Photo.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<StoreInfoDbContext>(options =>
        {
            _ = options.UseSqlServer(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString),
                builder => builder.MigrationsAssembly(typeof(StoreInfoDbContext).Assembly.FullName));
        }, contextLifetime: ServiceLifetime.Scoped);

        _ = services.AddDbContextFactory<StoreInfoDbContext>(options =>
        {
            _ = options.UseSqlServer(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString),
                builder => builder.MigrationsAssembly(typeof(StoreInfoDbContext).Assembly.FullName));
        }, ServiceLifetime.Transient);
        _ = services.AddScoped<StoreInfoDbContextSeed>();
        _ = services.AddTransient(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        _ = services.AddScoped(typeof(IMinIOResourceService), typeof(MinIOResourceService));
        _ = services.AddTransient(typeof(IStoreInfoRepository), typeof(StoreInfoRepository));
        _ = services.AddScoped(typeof(IBucketRepository), typeof(BucketRepository));
        _ = services.AddConfigurationSettingsThirdExtenal(configuration);
        _ = services.AddDapperForMSSQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        return services;
    }
}