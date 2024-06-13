using API.Intergration.Config.Service.Infrastructure.Endpoints;
using Infrastructure.Commons;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SerilogAction.Configure);

IConfiguration configuration = builder.Configuration;

Log.Information($"Starting {builder.Environment.ApplicationName} API up");
try
{
    IServiceCollection services = builder.Services;
    {
        _ = services.AddConfigurationSettings(configuration);

        _ = services.AddWorkContextAccessor();

        _ = services.AddScoped<ISerializeService, SerializeService>();

        builder.AddAppConfigurations();

    }

    WebApplication app = builder.Build();
    {
        app.AddMapGrpcServices();
    }

    _ = app.ConfigureEndpoints(configuration);

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