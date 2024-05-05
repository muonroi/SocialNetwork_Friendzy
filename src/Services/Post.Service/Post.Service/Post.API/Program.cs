using Commons.Logging;
using Post.API.Extentions;
using Post.Infrastructure.Persistence;
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
        PostDbContextSeed services = scope.ServiceProvider.GetRequiredService<PostDbContextSeed>();
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
    Log.Information("Shut down Post Service complete");
    Log.CloseAndFlush();
}