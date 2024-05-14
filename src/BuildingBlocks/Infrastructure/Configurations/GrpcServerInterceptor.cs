namespace Infrastructure.Configurations;

internal class GrpcServerInterceptor(Func<object?> workContextInfoFunc) : GrpcCoreInterceptor
{
    private readonly Func<object?> _workContextInfoFunc = workContextInfoFunc ?? throw new ArgumentNullException(nameof(workContextInfoFunc));

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Stopwatch watch = Stopwatch.StartNew();
        AsyncUnaryCall<TResponse>? result = default;

        AddCallerMetadata(ref context);
        AsyncUnaryCall<TResponse> call = continuation(request, context);
        result = new AsyncUnaryCall<TResponse>(
                   HandleResponseAsync(call.ResponseAsync),
                   call.ResponseHeadersAsync,
                   call.GetStatus,
                   call.GetTrailers,
                   call.Dispose);
        return result;
    }

    private static async Task<TResponse> HandleResponseAsync<TResponse>(Task<TResponse> t)
    {
        try
        {
            TResponse response = await t;
            return response;
        }
        catch (RpcException)
        {
            throw;
        }
    }

    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        AddCallerMetadata(ref context);
        return continuation(request, context);
    }

    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        AddCallerMetadata(ref context);
        return continuation(context);
    }

    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        AddCallerMetadata(ref context);
        return continuation(context);
    }

    public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        AddCallerMetadata(ref context);
        return continuation(request, context);
    }

    private void AddCallerMetadata<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context)
      where TRequest : class
      where TResponse : class
    {
        object? workContextInfoObj = _workContextInfoFunc.Invoke();
        Metadata grpcMetadata = [];
        WorkContextInfoDTO WorkContextInfo = Convert(workContextInfoObj);
        if (string.IsNullOrEmpty(WorkContextInfo.CorrelationId))
        {
            WorkContextInfo.CorrelationId = Guid.NewGuid().ToString();
        }

        grpcMetadata.Add(nameof(WorkContextInfo.CorrelationId), WorkContextInfo.CorrelationId);
        grpcMetadata.Add(nameof(WorkContextInfo.Caller), WorkContextInfo.Caller ?? string.Empty);
        grpcMetadata.Add(nameof(WorkContextInfo.ClientIpAddr), WorkContextInfo.ClientIpAddr ?? string.Empty);
        grpcMetadata.Add(nameof(WorkContextInfo.Username), WorkContextInfo.Username);
        grpcMetadata.Add("Accept-Language", WorkContextInfo.Language ?? "vi -VN");
        grpcMetadata.Add(nameof(WorkContextInfo.Roles), WorkContextInfo.Roles);
        grpcMetadata.Add(nameof(WorkContextInfo.AgentCode), WorkContextInfo.AgentCode);
        grpcMetadata.Add(nameof(WorkContextInfo.LinerCode), WorkContextInfo.LinerCode);
        grpcMetadata.Add(nameof(WorkContextInfo.UserId), WorkContextInfo.UserId.ToString());
        grpcMetadata.Add(nameof(WorkContextInfo.IsMasterAccount), WorkContextInfo.IsMasterAccount.ToString());
        grpcMetadata.Add(nameof(WorkContextInfo.RelatedAccounts), string.Join(",", WorkContextInfo.RelatedAccounts ?? []));

        CallOptions options = context.Options.WithHeaders(grpcMetadata);
        context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
    }

    private static WorkContextInfoDTO Convert(object? obj)
    {
        if (obj is null)
        {
            return new WorkContextInfoDTO();
        }

        WorkContextInfoDTO WorkContextInfo = new();
        PropertyInfo[] contextProperties = obj.GetType().GetProperties();

        foreach (PropertyInfo property in contextProperties)
        {
            if (!property.CanRead)
            {
                continue;
            }

            object? value = property.GetValue(obj);
            PropertyInfo? interceptorProperty = WorkContextInfo.GetType().GetProperty(property.Name);
            if (interceptorProperty is not null && interceptorProperty.CanWrite)
            {
                interceptorProperty?.SetValue(WorkContextInfo, value);
            }
        }

        return WorkContextInfo;
    }
}