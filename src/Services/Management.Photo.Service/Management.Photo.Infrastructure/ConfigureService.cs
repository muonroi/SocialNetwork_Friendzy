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