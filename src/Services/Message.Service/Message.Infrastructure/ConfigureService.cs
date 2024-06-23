namespace Message.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddSingleton<IMongoClient>(
        new MongoClient(GetConnectionMongoDbNameString(configuration)))
            .AddScoped(x => x.GetService<IMongoClient>()!.StartSession());
        _ = services.AddScoped<ISerializeService, SerializeService>();
        AgoraSetting? agoraSettings = configuration.GetSection(nameof(AgoraSetting)).Get<AgoraSetting>();

        if (agoraSettings != null)
        {
            agoraSettings.AppId = configuration.GetExCipherText(agoraSettings.AppId!);
            agoraSettings.AppCertificate = configuration.GetExCipherText(agoraSettings.AppCertificate!);
        }
        DatabaseConfigs? databaseSettings = configuration.GetSection(nameof(DatabaseConfigs)).Get<DatabaseConfigs>();

        _ = services.AddSingleton(agoraSettings!);
        _ = services.AddSingleton(databaseSettings!);
        return services;
    }

    private static string GetConnectionMongoDbNameString(this IConfiguration configuration)
    {
        string mongoDbConnectionString = configuration.GetConfigHelper(ConfigurationSetting.ConnectionString);
        string mongoDbName = configuration.GetConfigHelper(ConfigurationSetting.ConnectionMongoDbNameString);
        string result = mongoDbConnectionString + "/" + mongoDbName + "?authSource=admin";
        return result;
    }
}