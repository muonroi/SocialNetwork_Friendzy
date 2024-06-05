namespace Authenticate.Service.Extensions;

internal static class HostExtension
{
    internal static void AddMapGrpcServices(this WebApplication app)
    {
        _ = app.MapGrpcService<AuthenticateService>();
    }
}