using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace Message.Service.Infrastructure;

public static class SeedingDataConfig
{
    public static WebApplication SeedConfig(this WebApplication app, IConfiguration configuration)
    {
        using IServiceScope scope = app.Services.CreateScope();
        string mongoDbName = configuration.GetConfigHelper(ConfigurationSetting.ConnectionMongoDbNameString);
        IMongoClient mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
        ILogger logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        new MessageSeed(logger).SeedAsync(mongoClient, mongoDbName).Wait();
        return app;
    }
}