namespace API.Intergration.Config.Service.Extensions
{
    public static class HostExtension
    {
        internal static void AddMapGrpcServices(this WebApplication app)
        {
            _ = app.MapGrpcService<ApiIntergrationService>();
        }
    }
}