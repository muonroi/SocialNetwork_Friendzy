using Commons.Logging;
using Post.API.Extentions;
using Post.API.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

ConfigurationManager configuration = builder.Configuration;

builder.Host.UseSerilog(SerilogAction.Configure);

Log.Information($"Starting {builder.Environment.ApplicationName} API up");

try
{
    _ = services.AddConfigurationSettings(configuration);

    builder.AddAppConfigurations();

    WebApplication app = builder.Build();

    _ = app.SeedConfigAsync();

    app.AddMapGrpcServices();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down Post Service complete");
    Log.CloseAndFlush();
}