Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment env = builder.Environment;

ConfigurationManager configuration = builder.Configuration;

builder.Host.UseSerilog(SerilogAction.Configure);

Log.Information($"Starting {builder.Environment.ApplicationName} API up");
try
{
    ConsulConfigs consulSettings = ConsulConfigsExtensions.GetConfigs(configuration);

    IServiceCollection services = builder.Services;
    {
        _ = services.Configure<ConsulConfigs>(configuration.GetSection(nameof(ConsulConfigs)));

        _ = services.ConfigureJwtBearerToken(configuration);

        _ = services.AddControllers();

        _ = services.AddWorkContextAccessor();

        _ = services.AddConsul(consulSettings, env);

        _ = services.AddConfigurationSettings(configuration);

        _ = services.AddInfrastructureServices(configuration);

        _ = services.AddConfigurationApplication(configuration, env);

        _ = services.AddEndpointsApiExplorer();

        _ = services.SwaggerConfig(builder.Environment.ApplicationName);
    }

    builder.AddAppConfigurations();

    WebApplication app = builder.Build();
    {
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