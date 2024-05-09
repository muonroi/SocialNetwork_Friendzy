using Contracts.Commons.Constants;
using Contracts.Commons.Interfaces;
using Dapper.Extensions;
using Dapper.Extensions.MSSQL;
using Infrastructure.Commons;
using Infrastructure.Extensions;
using Infrastructure.Helper;
using Infrastructure.ORMs.Dapper;
using Management.Photo.Application.Commons.Interfaces;
using Management.Photo.Infrastructure.Persistences;
using Management.Photo.Infrastructure.Repository;
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
        });
        _ = services.AddScoped<StoreInfoDbContextSeed>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped(typeof(IStoreInfoRepository), typeof(StoreInfoRepository));
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        _ = services.AddDapperForMSSQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        return services;
    }
}