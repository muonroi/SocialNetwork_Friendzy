namespace Management.Photo.Service.Extensions;

public static class HostExtension
{
    public static void AddAppConfigurations(this WebApplicationBuilder builder)
    {
        _ = builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true,
                           reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}