﻿using Message.Application.Infrastructure.Hubs;

namespace Message.Service.Infrastructure.Endpoints;

internal static class EndpointConfigure
{
    internal static IApplicationBuilder ConfigureEndpoints(this WebApplication app, IConfiguration configuration)
    {
        _ = app.UseMiddleware<GlobalExceptionMiddleware>();

        _ = app.UseAuthenticationMiddleware(configuration);

        _ = app.UseWorkContext();

        _ = app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

        _ = app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        _ = app.MapHub<MessageHub>("message-hub").RequireAuthorization();

        return app;
    }
}