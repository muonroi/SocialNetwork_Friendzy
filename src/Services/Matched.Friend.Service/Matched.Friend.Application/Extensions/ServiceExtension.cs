namespace Matched.Friend.Application.Extensions;

public static class ServiceExtension
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
       IConfiguration configuration, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        _ = configuration.ToBase64();
        _ = services.AddInternalService();
        _ = services.AddTransient(typeof(GrpcConfigClientFactory<>));
        _ = services.AddGrpcClientServices(configuration, environment);
        _ = services.AddPaginationConfigs(configuration);
        _ = services.AddApiIntegration(configuration, httpContextAccessor);
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

    public static IServiceCollection AddApiIntegration(this IServiceCollection services, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _ = services.RegisterServiceForwarder<IApiExternalClient>(ApiPartnerConstants.PartnerName)
            .AddRestEaseMessageHandler(configuration, ApiPartnerConstants.ApiCode, ApiPartnerConstants.APIType, httpContextAccessor);
        return services;
    }

    private static IHttpClientBuilder AddRestEaseMessageHandler(this IHttpClientBuilder builder, IConfiguration configuration, string partnerCode, string partnerType, IHttpContextAccessor httpContextAccessor)
    {
        _ = builder.AddHttpMessageHandler(serviceProvider =>
        {
            IApiConfigSerivce apiConfigSerivce = serviceProvider.GetRequiredService<IApiConfigSerivce>();
            async Task<Dictionary<string, string>> _callbackApi(HttpRequestHeaders request)
            {
                string? secretKey = configuration.GetEx("SecretKey");
                IHeaderDictionary? token = httpContextAccessor.HttpContext?.Request.Headers;

                string? accessToken = token?.Authorization.FirstOrDefault(header => header?.StartsWith("Bearer ") == true)?["Bearer ".Length..];

                IWorkContextAccessor doWorkContext = serviceProvider.GetRequiredService<IWorkContextAccessor>();

                request.Add("Authorization", accessToken);
                request.Add("Request-Id", Guid.NewGuid().ToString());
                return await apiConfigSerivce.GetIntegrationApiAsync(partnerCode, partnerType);
            }
            return new RestEaseServiceDiscoveryMessageHandler(_callbackApi);
        });
        return builder;
    }

    #endregion Create API Integration
}