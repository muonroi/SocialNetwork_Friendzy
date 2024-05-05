namespace Commons.Logging;

public static class SerilogAction
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure => (context, configuration) =>
    {
        string? applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
        string environmentName = context.HostingEnvironment.EnvironmentName;
        _ = configuration
            .WriteTo.Debug()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Environment", environmentName)
            .Enrich.WithProperty("ApplicationName", applicationName)
            .ReadFrom.Configuration(context.Configuration);
    };
}