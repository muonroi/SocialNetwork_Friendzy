using Post.Service.Services;

namespace Post.Service.Extentions;

public static class HostExtension
{
    internal static void AddMapGrpcServices(this WebApplication app)
    {
        _ = app.MapGrpcService<PostService>();
    }
}