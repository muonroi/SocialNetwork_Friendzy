namespace Post.Service.Infrastructure.Endpoints;

internal static class EndpointConfigure
{
    internal static IApplicationBuilder ConfigureEndpoints(this WebApplication app, IConfiguration configuration)
    {
        _ = app.UseMiddleware<GlobalExceptionMiddleware>();

        //  _ = app.UseAuthenticationMiddleware(configuration); set apikey here

        _ = app.UseWorkContext();

        return app;
    }
}