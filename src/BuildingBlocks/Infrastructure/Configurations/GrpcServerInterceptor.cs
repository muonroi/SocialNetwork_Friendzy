using Shared.Models;

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
        WorkContextInfoModel WorkContextInfo = Convert(workContextInfoObj);
        if (string.IsNullOrEmpty(WorkContextInfo.CorrelationId))
        {
            WorkContextInfo.CorrelationId = Guid.NewGuid().ToString();
        }
        grpcMetadata.Add("Accept-Language", WorkContextInfo.Language ?? "vi-VN");
        grpcMetadata.Add(nameof(WorkContextInfoModel.CorrelationId), WorkContextInfo.CorrelationId);
        grpcMetadata.Add(nameof(WorkContextInfoModel.Caller), WorkContextInfo.Caller ?? string.Empty);
        grpcMetadata.Add(nameof(WorkContextInfoModel.ClientIpAddr), WorkContextInfo.ClientIpAddr ?? string.Empty);
        grpcMetadata.Add(nameof(WorkContextInfoModel.Username), WorkContextInfo.Username);
        grpcMetadata.Add(nameof(WorkContextInfoModel.Roles), WorkContextInfo.Roles);
        grpcMetadata.Add(nameof(WorkContextInfoModel.AgentCode), WorkContextInfo.AgentCode);
        grpcMetadata.Add(nameof(WorkContextInfoModel.UserId), WorkContextInfo.UserId.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoModel.FirstName), WorkContextInfo.FirstName);
        grpcMetadata.Add(nameof(WorkContextInfoModel.LastName), WorkContextInfo.LastName);
        grpcMetadata.Add(nameof(WorkContextInfoModel.PhoneNumber), WorkContextInfo.PhoneNumber);
        grpcMetadata.Add(nameof(WorkContextInfoModel.RoleIds), WorkContextInfo.RoleIds);
        grpcMetadata.Add(nameof(WorkContextInfoModel.EmailAddress), WorkContextInfo.EmailAddress);
        grpcMetadata.Add(nameof(WorkContextInfoModel.IsActive), WorkContextInfo.IsActive.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoModel.Balance), WorkContextInfo.Balance);
        grpcMetadata.Add(nameof(WorkContextInfoModel.IsEmailVerify), WorkContextInfo.IsEmailVerify.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoModel.AccountStatus), WorkContextInfo.AccountStatus.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoModel.Currency), WorkContextInfo.Currency.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoModel.AccountType), WorkContextInfo.AccountType.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoModel.IsAuthenticated), WorkContextInfo.IsAuthenticated.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoModel.Latitude), WorkContextInfo.Latitude.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoModel.Longitude), WorkContextInfo.Longitude.ToString());
        grpcMetadata.Add(nameof(WorkContextInfoModel.AccountId), WorkContextInfo.AccountId.ToString());

        CallOptions options = context.Options.WithHeaders(grpcMetadata);
        context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
    }

    private static WorkContextInfoModel Convert(object? obj)
    {
        if (obj is null)
        {
            return new WorkContextInfoModel();
        }

        WorkContextInfoModel WorkContextInfo = new();
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