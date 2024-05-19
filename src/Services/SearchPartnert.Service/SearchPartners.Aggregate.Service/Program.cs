using SearchPartners.Aggregate.Service.Infrastructure;

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

    _ = services.Configure<ConsulConfigs>(configuration.GetSection(nameof(ConsulConfigs)));

    ConsulConfigs consulSettings = ConsulConfigsExtensions.GetConfigs(configuration);

    // Add services to the container.
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
    services.ConfigureJwtBearerToken(configuration);

    _ = services.AddConfigurationApplication();

    _ = services.AddWorkContextAccessor();

    _ = services.AddConsul(consulSettings, env);

    _ = services.AddConfigurationSettings(configuration, env);

    builder.AddAppConfigurations();

    _ = services.AddEndpointsApiExplorer();

    services.SwaggerConfig();

    WebApplication app = builder.Build();

    _ = app.UseAuthentication();

    _ = app.UseAuthorization();

    _ = app.UseWorkContext();

    _ = app.UseConsul(consulSettings, env);

    EndpointConfigure.ConfigureEndpoints(app);
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
    Log.Information("Shut down SearchPartnersAggregate Service complete");

    Log.CloseAndFlush();
}