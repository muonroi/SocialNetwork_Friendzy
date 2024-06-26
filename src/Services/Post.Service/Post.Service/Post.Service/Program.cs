using Post.Application;
using Post.Service.Extentions;
using Post.Service.Infrastructure;
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

    _ = services.AddConfigurationApplication();

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