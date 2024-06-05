Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SerilogAction.Configure);

IServiceCollection services = builder.Services;

IConfiguration configuration = builder.Configuration;

Log.Information($"Starting {builder.Environment.ApplicationName} API up");
try
{
    _ = services.AddConfigurationSettings(configuration);

    builder.AddAppConfigurations();

    WebApplication app = builder.Build();

    _ = app.SeedConfigAsync();

    _ = app.UseMiddleware<GlobalExceptionMiddleware>();

    app.AddMapGrpcServices();

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
    Log.Information("Shut down Distance Service complete");
    Log.CloseAndFlush();
}