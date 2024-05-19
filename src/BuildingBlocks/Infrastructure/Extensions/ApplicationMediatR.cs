using Commons.Behaviours;
using MediatR;

namespace Infrastructure.Extensions;
public static class ApplicationMediatR
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, Assembly assembly)
    {
        _ = services.AddAutoMapper(assembly)
            .AddValidatorsFromAssembly(assembly)
            .AddMediatR(x => x.RegisterServicesFromAssemblies(assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviours<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        _ = services.AddTransient<Stopwatch>();

        return services;
    }
}
