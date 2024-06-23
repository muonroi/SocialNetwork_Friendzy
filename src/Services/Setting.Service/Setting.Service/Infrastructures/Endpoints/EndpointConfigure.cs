namespace Setting.Service.Infrastructures.Endpoints;

internal static class EndpointConfigure
{
    internal static IApplicationBuilder ConfigureEndpoints(this WebApplication app, IConfiguration configuration)
    {
        _ = app.UseMiddleware<GlobalExceptionMiddleware>();

        // _ = app.UseAuthenticationMiddleware(configuration);

        _ = app.UseWorkContext();

        _ = app.MapControllerRoute(
                           name: "default",
                                          pattern: "{controller=Home}/{action=Index}/{id?}");
        _ = app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        return app;
    }
}