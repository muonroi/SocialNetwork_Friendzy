using Account.Application.Helper;

namespace Account.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<AccountDbContext>(options =>
        {
            _ = options.UseSqlServer(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString),
                builder => builder.MigrationsAssembly(typeof(AccountDbContext).Assembly.FullName));
        });
        _ = services.AddScoped<AccountDbContextSeed>();
        _ = services.AddSingleton<PresenceTracker>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        _ = services.AddScoped<IAccountRepository, AccountRepository>();
        _ = services.AddScoped<IAccountRoleRepository, AccountRoleRepository>();
        _ = services.AddScoped<ISerializeService, SerializeService>();
        return services;
    }
}