namespace Infrastructure.Commons;

public class ConsulServiceDiscoveryMessageHandler(IConsulClient? consulClient, IWebHostEnvironment environment) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Uri? current = request.RequestUri;
        try
        {
            if (!environment.IsDevelopment())
            {
                string serviceName = $"{current?.Host}";
                ArgumentNullException.ThrowIfNull(consulClient);
                string url = await consulClient.GetUriOnConsulAsync(serviceName);
                Uri uri = new(url);
                request.RequestUri = new Uri(uri, current?.PathAndQuery);
            }

            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            request.RequestUri = current;
        }
    }
}