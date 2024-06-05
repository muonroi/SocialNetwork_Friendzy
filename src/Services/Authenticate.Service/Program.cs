Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SerilogAction.Configure);

IServiceCollection services = builder.Services;

Log.Information($"Starting {builder.Environment.ApplicationName} API up");
try
{
    _ = services.AddConfigurationSettings();

    builder.AddAppConfigurations();

    WebApplication app = builder.Build();

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
    Log.Information("Shut down Authenticate Service complete");
    Log.CloseAndFlush();
}