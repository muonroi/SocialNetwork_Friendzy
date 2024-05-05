using Commons.Logging;
using Distance.Service.Extensions;
using Distance.Service.Persistences;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SerilogAction.Configure);
Log.Information($"Starting {builder.Environment.ApplicationName} API up");
try
{
    _ = builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.AddAppConfigurations();

    WebApplication app = builder.Build();

    using (IServiceScope scope = app.Services.CreateScope())
    {
        DistanceDbContextSeed services = scope.ServiceProvider.GetRequiredService<DistanceDbContextSeed>();
        await services.InitialiseAsync();
        await services.SeedAsync();
    }

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