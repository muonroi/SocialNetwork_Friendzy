using Commons.Logging;
using Management.Photo.Application;
using Management.Photo.Infrastructure;
using Management.Photo.Service.Extensions;
using Management.Photo.Service.Infrastructures;
using Management.Photo.Service.Infrastructures.Endpoints;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

IWebHostEnvironment env = builder.Environment;

ConfigurationManager configuration = builder.Configuration;

builder.Host.UseSerilog(SerilogAction.Configure);

Log.Information($"Starting {builder.Environment.ApplicationName} API up");
try
{
    // Add services to the container.
    _ = services.AddControllers();

    _ = services.AddConfigurationSettings(configuration);

    _ = services.AddInfrastructureServices(configuration);

    _ = services.AddApplicationServices();

    builder.AddAppConfigurations();

    _ = services.AddEndpointsApiExplorer();

    _ = services.AddSwaggerGen();

    WebApplication app = builder.Build();

    _ = app.SeedConfigAsync();

    _ = app.UseAuthorization();

    app.ConfigureEndpoints();

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
    Log.Information("Shut down Management Photo Service complete");
    Log.CloseAndFlush();
}