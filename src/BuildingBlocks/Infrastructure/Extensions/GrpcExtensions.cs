namespace Infrastructure.Extensions;

public static class GrpcExtensions
{
    public static void AddGrpcServer(this IServiceCollection services)
    {
        _ = services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxSendMessageSize = 100 * 1024 * 1024;
            options.MaxReceiveMessageSize = 100 * 1024 * 1024;
            options.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            options.ResponseCompressionAlgorithm = "gzip";
            _ = options.EnableMessageValidation();
        });
        _ = services.AddGrpcValidation();
    }

    public static IServiceCollection AddGrpcClientDelegate(this IServiceCollection services, Func<object?> workContextFunc)
    {
        ArgumentNullException.ThrowIfNull(workContextFunc);
        services.Add(ServiceDescriptor.Describe(typeof(GrpcClientInterceptor),
            sp => ActivatorUtilities.CreateInstance<GrpcClientInterceptor>(sp, [workContextFunc]), ServiceLifetime.Scoped));

        return services;
    }

    public static IHttpClientBuilder AddGrpcClientInterceptor<
    TClient>(this IServiceCollection services
           , Dictionary<string, string> grpcServiceOptions
           , string serviceName, IWebHostEnvironment environment) where TClient : class
    {
        string serviceUri = ServiceUri(grpcServiceOptions, serviceName, environment);
        return services.AddGrpcClient<TClient>(serviceName, o =>
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            o.Address = new Uri(serviceUri);
            MethodConfig defaultMethodConfig = new()
            {
                Names = { MethodName.Default },
                RetryPolicy = new RetryPolicy
                {
                    MaxAttempts = 2,
                    InitialBackoff = TimeSpan.FromSeconds(3),
                    MaxBackoff = TimeSpan.FromSeconds(3),
                    BackoffMultiplier = 1,
                    RetryableStatusCodes =
                        {
                           StatusCode.NotFound,
                           StatusCode.Unavailable,
                        }
                }
            };
            o.ChannelOptionsActions.Add(options =>
            {
                options.DisposeHttpClient = true;
                options.MaxSendMessageSize = 100 * 1024 * 1024;
                options.MaxReceiveMessageSize = 100 * 1024 * 1024;
                options.ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } };
            });
        })
         .EnableCallContextPropagation(
             o => o.SuppressContextNotFoundErrors = true
         )
         .ConfigurePrimaryHttpMessageHandler(() =>
         {
             HttpClientHandler handler = new()
             {
                 MaxConnectionsPerServer = 300,
                 AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                 ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
             };
             return handler;
         })
         .SetHandlerLifetime(TimeSpan.FromMinutes(5))
         .AddInterceptor<GrpcClientInterceptor>(InterceptorScope.Client);
    }

    private static string ServiceUri(Dictionary<string, string> grpcServiceOptions, string serviceName, IWebHostEnvironment environment)
    {
        string defaultProtocol = "http";

        if (grpcServiceOptions.TryGetValue(serviceName, out string? serviceUri))
        {
            if (!environment.IsDevelopment())
            {
                serviceUri = $"{defaultProtocol}://{serviceUri}";
            }
        }
        return string.IsNullOrEmpty(serviceUri)
            ? throw new KeyNotFoundException($"Service '{serviceName}' not found in grpcServiceOptions dictionary.")
            : serviceUri;
    }



    public static string GetByKey(this Metadata metadata, string key)
    {
        key = key.Trim();
        if (string.IsNullOrWhiteSpace(key) || metadata is null)
        {
            return string.Empty;
        }

        string? value = metadata.GetValue(key);
        return value ?? string.Empty;
    }
}