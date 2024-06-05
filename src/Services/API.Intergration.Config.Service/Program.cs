Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SerilogAction.Configure);

IServiceCollection services = builder.Services;

IWebHostEnvironment env = builder.Environment;

Log.Information($"Starting {builder.Environment.ApplicationName} API up");
try
{
    _ = services.Configure<ConsulConfigs>(builder.Configuration.GetSection(nameof(ConsulConfigs)));

    ConsulConfigs consulSettings = ConsulConfigsExtensions.GetConfigs(builder.Configuration);

    _ = services.AddConfigurationSettings(builder.Configuration);

    builder.AddAppConfigurations();

    _ = services.AddConsul(consulSettings, env);

    _ = services.AddWorkContextAccessor();

    WebApplication app = builder.Build();

    _ = app.UseWorkContext();

    _ = app.UseConsul(consulSettings, env);

    app.AddMapGrpcServices();
    _ = app.UseMiddleware<GlobalExceptionMiddleware>();
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
    Log.Information("Shut down Api intergration Service complete");
    Log.CloseAndFlush();
}