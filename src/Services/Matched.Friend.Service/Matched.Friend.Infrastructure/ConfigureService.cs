using Dapper.Extensions.MySql;

namespace Matched.Friend.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<FriendsMatchedDbContext>(options =>
        {
            _ = options.UseMySQL(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString),
                builder => builder.MigrationsAssembly(typeof(FriendsMatchedDbContext).Assembly.FullName));
        });
        _ = services.AddHttpContextAccessor();
        _ = services.AddDapperForMySQL();
        _ = services.AddScoped<FriendsMatchedDbContextSeed>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        _ = services.AddScoped<IFriendsMatchedRepository, FriendsMatchedRepository>();
        return services;
    }
}