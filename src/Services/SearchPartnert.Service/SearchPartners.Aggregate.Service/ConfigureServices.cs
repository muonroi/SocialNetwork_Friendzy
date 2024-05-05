﻿using FluentValidation;
using MediatR;
using SearchPartners.Aggregate.Service.Infrastructure.Behaviours;
using System.Diagnostics;
using System.Reflection;

namespace SearchPartners.Aggregate.Service;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        _ = services.AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviours<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        _ = services.AddTransient<Stopwatch>();

        return services;
    }
}