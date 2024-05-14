namespace Post.Service.Infrastructure.Endpoints;

internal static class EndpointConfigure
{
    internal static void ConfigureEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI();
        }
        _ = app.UseAuthorization();
        _ = app.MapControllers();
        _ = app.UseCors();
        _ = app.MapControllerRoute(
                           name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
        _ = app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });
        app.Run();
    }
}