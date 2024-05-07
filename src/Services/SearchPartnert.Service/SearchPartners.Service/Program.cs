Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SerilogAction.Configure);

IServiceCollection services = builder.Services;

IWebHostEnvironment env = builder.Environment;

ConfigurationManager configuration = builder.Configuration;

Log.Information($"Starting {builder.Environment.ApplicationName} API up");

try
{
    // Add services to the container.
    _ = services.AddControllers();

    _ = services.AddConfigurationSettings(configuration);

    builder.AddAppConfigurations();

    _ = services.AddEndpointsApiExplorer();

    _ = services.AddSwaggerGen();

    WebApplication app = builder.Build();

    app.AddMapGrpcServices();

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
    Log.Information("Shut down SearchPartners Service complete");

    Log.CloseAndFlush();
}