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

    _ = services.AddConfigurationApplication(configuration, env);

    _ = services.AddEndpointsApiExplorer();

    services.SwaggerConfig();

    builder.AddAppConfigurations();

    WebApplication app = builder.Build();

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