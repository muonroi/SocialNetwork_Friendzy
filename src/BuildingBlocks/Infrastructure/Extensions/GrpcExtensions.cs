namespace Infrastructure.Extensions;

public static class GrpcExtensions
{
    public static void AddGrpcServer(this IServiceCollection services)
    {
        // services.AddSingleton<GrpcServerInterceptor>();
        _ = services.AddGrpc(options =>
        {
            // options.Interceptors.Add<GrpcServerInterceptor>();
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

    public static void AddGrpcClientInterceptor2<
    TClient>(this IServiceCollection services
           , Dictionary<string, string> grpcServiceOptions
           , string serviceName, IWebHostEnvironment environment)
    {
        _ = ServiceUri(grpcServiceOptions, serviceName, environment);

        //return services.AddGrpcClient<TClient>(serviceName, o =>
        //{
        //    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        //    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
        //    o.Address = new Uri(serviceUri);
        //    MethodConfig defaultMethodConfig = new()
        //    {
        //        Names = { MethodName.Default },
        //        RetryPolicy = new RetryPolicy
        //        {
        //            MaxAttempts = 2,
        //            InitialBackoff = TimeSpan.FromSeconds(3),
        //            MaxBackoff = TimeSpan.FromSeconds(3),
        //            BackoffMultiplier = 1,
        //            RetryableStatusCodes =
        //                {
        //                   StatusCode.NotFound,
        //                   StatusCode.Unavailable,
        //                }
        //        }
        //    };
        //    o.ChannelOptionsActions.Add(options =>
        //    {
        //        options.DisposeHttpClient = true;
        //        options.MaxSendMessageSize = 100 * 1024 * 1024;
        //        options.MaxReceiveMessageSize = 100 * 1024 * 1024;
        //        options.ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } };
        //    });
        //})
        // .EnableCallContextPropagation(
        //     o => o.SuppressContextNotFoundErrors = true
        // )
        // .ConfigurePrimaryHttpMessageHandler(() =>
        // {
        //     HttpClientHandler handler = new()
        //     {
        //         MaxConnectionsPerServer = 300,
        //         AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        //         ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        //     };
        //     return handler;
        // })
        // .SetHandlerLifetime(TimeSpan.FromMinutes(5))
        // .AddInterceptor<GrpcClientInterceptor>(InterceptorScope.Client);
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

    public static string GetParams<TRequest>(this TRequest request)
    {
        try
        {
            if (request is null)
            {
                return string.Empty;
            }

            string param = JsonConvert.SerializeObject(request);
            return param.Length > 1024 ? "Length=" + param.Length + " ### " + param[..1024] : param;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static string GetReturnValues(this object? returnValues)
    {
        try
        {
            if (returnValues is null)
            {
                return string.Empty;
            }

            System.Type type = returnValues.GetType();

            if (type is not null && returnValues is ICollection sourceCollection)
            {
                int count = sourceCollection.Count;
                if (count > 1)
                {
                    return string.Format("ItemCount: {0}, ", count);
                }
            }
            else if (type is not null && type.IsGenericType && type.Namespace == typeof(IEnumerable<>).Namespace && returnValues is IEnumerable<object> dataSource)
            {
                int count = dataSource.Count();
                if (count > 1)
                {
                    return string.Format("ItemCount: {0}, ", count);
                }
            }
            string retValue = JsonConvert.SerializeObject(returnValues);
            return retValue.Length > 2048 ? "Length=" + retValue.Length + " ### " + retValue[..2048] : retValue;
        }
        catch
        {
            return string.Empty;
        }
    }
}