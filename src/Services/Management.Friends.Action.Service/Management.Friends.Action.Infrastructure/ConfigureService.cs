using Dapper.Extensions.MySql;
using Management.Friends.Action.Application.Commons.Interfaces;
using Management.Friends.Action.Infrastructure.Persistence;
using Management.Friends.Action.Infrastructure.Repository;

namespace Management.Friends.Action.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<ManagementFriendsActionDbContext>(options =>
        {
            _ = options.UseMySQL(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString),
                builder => builder.MigrationsAssembly(typeof(ManagementFriendsActionDbContext).Assembly.FullName));
        });
        _ = services.AddHttpContextAccessor();
        _ = services.AddDapperForMySQL();
        _ = services.AddDapperConnectionStringProvider<ConnectionStringProvider>();
        _ = services.AddDapperCaching(configuration);
        _ = services.AddScoped<ManagementFriendsActionDbContextSeed>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        _ = services.AddScoped<IFriendsActionRepository, FriendsMatchedRepository>();
        return services;
    }
}