using Account.Application;
using Account.Application.Infrastructure;
using Account.Infrastructure;
using Account.Service.Extensions;
using Account.Service.Infrastructures;
using Account.Service.Infrastructures.Endpoints;
using Commons.Logging;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
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
    // config appsetting
    _ = services.Configure<ConsulConfigs>(configuration.GetSection(nameof(ConsulConfigs)));

    ConsulConfigs consulSettings = ConsulConfigsExtensions.GetConfigs(configuration);
    // Add services to the container.

    _ = services.AddControllers();

    _ = services.AddWorkContextAccessor();

    _ = services.AddConsul(consulSettings, env);

    _ = services.AddConfigurationSettings(configuration);

    _ = services.AddInfrastructureServices(configuration);

    _ = services.AddConfigurationApplication(configuration, env);

    _ = services.AddEndpointsApiExplorer();

    _ = services.AddSwaggerGen();

    _ = services.AuthorizationRoles();

    builder.AddAppConfigurations();

    WebApplication app = builder.Build();

    _ = app.SeedConfigAsync();

    _ = app.UseWorkContext();

    _ = app.UseConsul(consulSettings, env);

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
    Log.Information("Shut down Account Service complete");
    Log.CloseAndFlush();
}