namespace Management.Photo.Service.Infrastructures.Endpoints;

internal static class EndpointConfigure
{
    internal static IApplicationBuilder ConfigureEndpoints(this WebApplication app, IConfiguration configuration, IWebHostEnvironment env)
    {
        _ = app.UseMiddleware<GlobalExceptionMiddleware>();

        _ = app.UseAuthenticationMiddleware(configuration);

        _ = app.UseWorkContext();
        _ = app.UseCors();
        _ = app.MapControllerRoute(
                           name: "default",
                                          pattern: "{controller=Home}/{action=Index}/{id?}");
        _ = app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        _ = app.MapGet("images/{filename}", async context =>
        {
            object? filename = context.Request.RouteValues["filename"];
            string file = Path.Combine(env.WebRootPath, "images", filename?.ToString()!);
            if (File.Exists(file))
            {
                await context.Response.SendFileAsync(file);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }).AllowAnonymous();
        return app;
    }
}