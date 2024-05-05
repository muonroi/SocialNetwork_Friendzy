namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorkContextAccessor(this IServiceCollection services)
    {
        _ = services.AddSingleton<IWorkContextAccessor, WorkContextAccessor>();
        return services;
    }
}