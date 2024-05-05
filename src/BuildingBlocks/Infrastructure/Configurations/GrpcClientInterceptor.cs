using Shared.DTOs;

namespace Infrastructure.Configurations;

public class GrpcClientInterceptor(Func<object?> workContextFunc) : GrpcCoreInterceptor
{
    private readonly Func<object?> _workContextFunc = workContextFunc ?? throw new ArgumentNullException(nameof(workContextFunc));

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
        object? workContextObj = _workContextFunc.Invoke();
        Metadata grpcMetadata = [];
        WorkContextInfoDTO workContext = Convert(workContextObj);
        if (string.IsNullOrEmpty(workContext.CorrelationId))
        {
            workContext.CorrelationId = Guid.NewGuid().ToString();
        }

        grpcMetadata.Add(nameof(WorkContextInfoDTO.CorrelationId), workContext.CorrelationId);
        grpcMetadata.Add(nameof(WorkContextInfoDTO.Caller), workContext.Caller ?? string.Empty);
        grpcMetadata.Add(nameof(WorkContextInfoDTO.ClientIpAddr), workContext.ClientIpAddr ?? string.Empty);
        grpcMetadata.Add(nameof(WorkContextInfoDTO.Username), workContext.Username);
        grpcMetadata.Add("Accept-Language", workContext.Language ?? "vi -VN");
        grpcMetadata.Add(nameof(WorkContextInfoDTO.Roles), workContext.Roles);
        grpcMetadata.Add(nameof(WorkContextInfoDTO.AgentCode), workContext.AgentCode);
        grpcMetadata.Add(nameof(WorkContextInfoDTO.LinerCode), workContext.LinerCode);
        grpcMetadata.Add(nameof(WorkContextInfoDTO.UserId), workContext.UserId.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoDTO.IsMasterAccount), workContext.IsMasterAccount.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoDTO.RelatedAccounts), string.Join(",", workContext.RelatedAccounts ?? []));

        CallOptions options = context.Options.WithHeaders(grpcMetadata);
        context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
    }

    private static WorkContextInfoDTO Convert(object? obj)
    {
        if (obj is null)
        {
            return new WorkContextInfoDTO();
        }

        WorkContextInfoDTO workContext = new();
        System.Reflection.PropertyInfo[] contextProperties = obj.GetType().GetProperties();

        foreach (System.Reflection.PropertyInfo property in contextProperties)
        {
            if (!property.CanRead)
            {
                continue;
            }

            object? value = property.GetValue(obj);
            System.Reflection.PropertyInfo? interceptorProperty = workContext.GetType().GetProperty(property.Name);
            if (interceptorProperty is not null && interceptorProperty.CanWrite)
            {
                interceptorProperty?.SetValue(workContext, value);
            }
        }

        return workContext;
    }
}