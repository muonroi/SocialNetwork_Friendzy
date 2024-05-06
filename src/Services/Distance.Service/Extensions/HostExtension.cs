

namespace Distance.Service.Extensions;

internal static class HostExtension
{
    internal static void AddAppConfigurations(this WebApplicationBuilder builder)
    {
        _ = builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true,
                           reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    internal static void AddMapGrpcServices(this WebApplication app)
    {
        _ = app.MapGrpcService<DistanceServices>();
    }
}