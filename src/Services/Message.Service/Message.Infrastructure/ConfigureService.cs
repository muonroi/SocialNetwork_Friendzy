namespace Message.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        MongoClient mongoClient = new(configuration.GetConfigHelper(ConfigurationSetting.ConnectionString));

        MessageDbContext.Create(mongoClient.GetDatabase(ConfigurationSetting.ConnectionMongoDbNameString));

        _ = services.AddScoped<MessageDbContextSeed>();

        _ = services.AddScoped<ISerializeService, SerializeService>();
        return services;
    }
}