using User.Service.Extensions;
using User.Service.Infrastructures;
using User.Service.Infrastructures.Endpoints;

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

    // Add services to the container.
    _ = services.AddControllers();

    _ = services.AddConfigurationSettings(configuration);

    _ = services.AddInfrastructureServices(configuration);

    _ = services.AddConfigurationApplication();

    _ = services.AddEndpointsApiExplorer();

    _ = services.AddSwaggerGen();

    builder.AddAppConfigurations();

    WebApplication app = builder.Build();

    _ = app.SeedConfigAsync();

    _ = app.UseAuthorization();

    app.ConfigureEndpoints();

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