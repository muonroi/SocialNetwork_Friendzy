using Calzolari.Grpc.AspNetCore.Validation.Internal;
using Consul;
using Contracts.Commons.Constants;
using Contracts.Commons.Interfaces;
using Infrastructure.Commons;
using Infrastructure.Extensions;
using Infrastructure.Factorys;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Authenticate.Verify.Service.AuthenticateVerify;

namespace Account.Application.Commons.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        _ = configuration.ToBase64();
        _ = services.AddTransient(typeof(GrpcConfigClientFactory<>));
        _ = services.AddGrpcClientServices(configuration, environment);
        _ = services.AddConfigurationSettingsThirdExtenal(configuration);
        return services;
    }

    #region Create Grpc Client service

    public static IServiceCollection AddGrpcClientServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        GrpcServiceOptions grpcServiceOptions = [];
        configuration.GetSection(nameof(GrpcServiceOptions)).Bind(grpcServiceOptions);

        ServiceProvider serviceProvider = services.BuildServiceProvider();
        _ = services.AddGrpcClientDelegate(() =>
        {
            IWorkContextAccessor doWorkContext = serviceProvider.GetRequiredService<IWorkContextAccessor>();
            return doWorkContext.WorkContext;
        });
        _ = services.AddGrpcClientInterceptor<AuthenticateVerifyClient>(grpcServiceOptions, ServiceConstants.AuthenticateService, environment)
              .AddConsulMessageHandler(environment);

        return services;
    }

    private static IHttpClientBuilder AddConsulMessageHandler(this IHttpClientBuilder builder, IWebHostEnvironment environment)
    {
        _ = builder.AddHttpMessageHandler(serviceProvider =>
        {
            IConsulClient? consulClient = serviceProvider.GetService<IConsulClient>();
            IWorkContextAccessor doWorkContext = serviceProvider.GetRequiredService<IWorkContextAccessor>();

            return new ConsulServiceDiscoveryMessageHandler(consulClient, environment);
        });
        return builder;
    }

    #endregion Create Grpc Client service
}