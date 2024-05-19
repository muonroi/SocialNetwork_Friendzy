using Commons.Logging;
using Infrastructure.Extensions;
using Management.Photo.Application;
using Management.Photo.Infrastructure;
using Management.Photo.Service.Infrastructures;
using Management.Photo.Service.Infrastructures.Endpoints;
using Serilog;
using System.Reflection;
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
    Assembly assemblyInstance = Assembly.GetExecutingAssembly();

    // Add services to the container.
    _ = services.AddControllers();

    _ = services.AddInfrastructureServices(configuration);

    _ = services.AddConfigurationApplication();

    _ = services.AddEndpointsApiExplorer();

    _ = services.AddSwaggerGen();

    _ = services.AddWorkContextAccessor();

    builder.AddAppConfigurations();

    WebApplication app = builder.Build();

    _ = app.UseWorkContext();

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