using Infrastructure.Configurations;
using User.Application.Infrastructure;
using User.Service.Extensions;
using User.Service.Infrastructures;
using User.Service.Infrastructures.Endpoints;

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
    _ = services.ConfigureJwtBearerToken(configuration);

    _ = services.AddControllers();

    _ = services.AddWorkContextAccessor();

    _ = services.AddConsul(consulSettings, env);

    _ = services.AddConfigurationSettings(configuration);

    _ = services.AddInfrastructureServices(configuration);

    _ = services.AddConfigurationApplication(configuration, env);

    _ = services.AddEndpointsApiExplorer();

    services.SwaggerConfig();

    _ = services.AuthorizationRoles();


    builder.AddAppConfigurations();

    WebApplication app = builder.Build();

    _ = app.SeedConfigAsync();

    if (app.Environment.IsDevelopment())
    {
        _ = app.UseSwagger();
        _ = app.UseSwaggerUI();
    }
    _ = app.MapControllers();

    _ = app.UseCors();

    _ = app.ConfigureEndpoints(configuration);

    _ = app.UseConsul(consulSettings, env);

    _ = app.UseAuthentication();

    _ = app.UseAuthorization();

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
    Log.Information("Shut down User Service complete");
    Log.CloseAndFlush();
}