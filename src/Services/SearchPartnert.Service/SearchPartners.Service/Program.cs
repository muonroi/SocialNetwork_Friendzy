using Commons.Logging;
using SearchPartners.Service.Extensions;
using SearchPartners.Service.Infrastructure.Endpoints;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SerilogAction.Configure);
Log.Information($"Starting {builder.Environment.ApplicationName} API up");
try
{
    // Add services to the container.
    _ = builder.Services.AddControllers();
    _ = builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.AddAppConfigurations();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    _ = builder.Services.AddEndpointsApiExplorer();
    _ = builder.Services.AddSwaggerGen();

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