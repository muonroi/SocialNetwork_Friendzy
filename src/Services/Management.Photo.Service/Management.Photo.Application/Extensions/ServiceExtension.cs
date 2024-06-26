﻿using Calzolari.Grpc.AspNetCore.Validation.Internal;
using Commons.Pagination;
using Consul;
using Contracts.Commons.Constants;
using Contracts.Commons.Interfaces;
using ExternalAPI;
using Infrastructure.Commons;
using Infrastructure.Extensions;
using Infrastructure.Factorys;
using Management.Photo.Application.Feature.v1.ApiConfigService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using static API.Intergration.Config.Service.Protos.ApiConfigGrpc;
using static Authenticate.Verify.Service.AuthenticateVerify;

namespace Management.Photo.Application.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
       IConfiguration configuration, IWebHostEnvironment environment)
    {
        _ = configuration.ToBase64();
        _ = services.AddInternalService();
        _ = services.AddTransient(typeof(GrpcConfigClientFactory<>));
        _ = services.AddGrpcClientServices(configuration, environment);
        _ = services.AddPaginationConfigs(configuration);
        _ = services.AddApiIntegration(configuration);
        return services;
    }

    #region Pagination

    public static IServiceCollection AddPaginationConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        PaginationConfigs paginationConfigs = new();
        configuration.GetSection(PaginationConfigs.SectionName).Bind(paginationConfigs);
        _ = services.AddSingleton(paginationConfigs);
        return services;
    }

    #endregion Pagination

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
        _ = services.AddGrpcClientInterceptor<ApiConfigGrpcClient>(grpcServiceOptions, ServiceConstants.ApiConfigService, environment)
            .AddConsulMessageHandler(environment);
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

    #region Internal service

    private static IServiceCollection AddInternalService(this IServiceCollection services)
    {
        _ = services.AddScoped<IApiConfigSerivce, ApiConfigService>();
        return services;
    }

    #endregion Internal service

    #region Create API Integration

    public static IServiceCollection AddApiIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.RegisterServiceForwarder<IApiExternalClient>(ApiPartnerConstants.PartnerName)
            .AddRestEaseMessageHandler(configuration, ApiPartnerConstants.ApiCode, ApiPartnerConstants.APIType);
        return services;
    }

    private static IHttpClientBuilder AddRestEaseMessageHandler(this IHttpClientBuilder builder, IConfiguration configuration, string partnerCode, string partnerType)
    {
        _ = builder.AddHttpMessageHandler(serviceProvider =>
        {
            IApiConfigSerivce apiConfigSerivce = serviceProvider.GetRequiredService<IApiConfigSerivce>();
            async Task<Dictionary<string, string>> _callbackApi(HttpRequestHeaders request)
            {
                string? secretKey = configuration.GetEx("SecretKey");
                string? apiKey = configuration.GetEx("ApiKey", secretKey!);
                IWorkContextAccessor doWorkContext = serviceProvider.GetRequiredService<IWorkContextAccessor>();

                request.Add("API-Key", apiKey);
                request.Add("Request-Id", Guid.NewGuid().ToString());
                return await apiConfigSerivce.GetIntegrationApiAsync(partnerCode, partnerType);
            }
            return new RestEaseServiceDiscoveryMessageHandler(_callbackApi);
        });
        return builder;
    }

    #endregion Create API Integration
}