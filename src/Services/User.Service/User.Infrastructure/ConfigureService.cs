using Contracts.Commons.Constants;

namespace User.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<UserDbContext>(options =>
        {
            _ = options.UseSqlServer(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString),
                builder => builder.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName));
        });
        _ = services.AddScoped<UserDbContextSeed>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped<IUserRepository, UserRepository>();
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        return services;
    }
}