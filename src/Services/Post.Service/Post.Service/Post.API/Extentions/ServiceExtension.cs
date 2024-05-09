using Contracts.Commons.Constants;

namespace Post.Service.Extentions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = services.AddDbContext<PostDbContext>(options =>
        {
            _ = options.UseMySQL(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString),
                builder => builder.MigrationsAssembly(typeof(PostDbContext).Assembly.FullName));
        });
        _ = services.AddScoped<PostDbContextSeed>();
        _ = services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>));
        _ = services.AddScoped<IPostRepository, PostRepository>();
        _ = services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        _ = ServiceExtensionCommon.AddConfigurationSettingsCommon(services, configuration);
        _ = services.AddDapperForMySQL();
        services.AddGrpcServer();
        return services;
    }
}