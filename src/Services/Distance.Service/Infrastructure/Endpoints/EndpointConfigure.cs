namespace Distance.Service.Infrastructure.Endpoints;

internal static class EndpointConfigure
{
    internal static IApplicationBuilder ConfigureEndpoints(this WebApplication app)
    {
        _ = app.UseMiddleware<GlobalExceptionMiddleware>();

        _ = app.UseWorkContext();

        return app;
    }
}