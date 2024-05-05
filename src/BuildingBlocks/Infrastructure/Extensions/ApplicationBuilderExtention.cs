namespace Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseWorkContext(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<WorkContextMiddleware>();
    }
}