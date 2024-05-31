using Commons.Logging;
using Infrastructure.Configurations;
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

    // config appsetting
    _ = services.Configure<ConsulConfigs>(configuration.GetSection(nameof(ConsulConfigs)));

    ConsulConfigs consulSettings = ConsulConfigsExtensions.GetConfigs(configuration);
    // Add services to the container.
    _ = services.ConfigureJwtBearerToken(configuration);

    _ = services.AddControllers();

    _ = services.AddWorkContextAccessor();

    _ = services.AddConsul(consulSettings, env);

    _ = services.AddInfrastructureServices(configuration);

    _ = services.AddConfigurationApplication(configuration, env);

    _ = services.AddEndpointsApiExplorer();

    _ = services.AddSwaggerGen();

    _ = services.AddWorkContextAccessor();

    builder.AddAppConfigurations();

    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        _ = app.UseSwagger();
        _ = app.UseSwaggerUI();
    }
    _ = app.MapControllers();

    _ = app.SeedConfigAsync();

    _ = app.UseAuthorization();

    _ = app.ConfigureEndpoints(configuration);

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