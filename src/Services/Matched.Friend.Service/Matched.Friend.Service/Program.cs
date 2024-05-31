using Commons.Logging;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using Matched.Friend.Application;
using Matched.Friend.Application.Infrastructure;
using Matched.Friend.Infrastructure;
using Matched.Friend.Service.Extensions;
using Matched.Friend.Service.Infrastructures;
using Matched.Friend.Service.Infrastructures.Endpoints;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    _ = services.ConfigureJwtBearerToken(configuration);

    _ = services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());

        options.SerializerSettings.Converters.Add(new CustomUnixDateTimeConverter());
    });
    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
    {
        Converters =
                [
                    new StringEnumConverter(),

                   new CustomUnixDateTimeConverter()
                ]
    };

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
    Log.Information("Shut down Friend Matched Service complete");
    Log.CloseAndFlush();
}