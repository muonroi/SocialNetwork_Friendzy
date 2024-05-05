using Consul;

namespace Infrastructure.Commons
{
    public class ConsulServiceDiscoveryMessageHandler(IConsulClient? consulClient, IWebHostEnvironment environment,
    Func<string> doTenantId) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Uri? current = request.RequestUri;
            try
            {
                string _tenantId = doTenantId.Invoke();
                if (!environment.IsDevelopment())
                {
                    string serviceName = $"{current?.Host}";
                    ArgumentNullException.ThrowIfNull(consulClient);
                    string url = await consulClient.GetUriOnConsulAsync(serviceName, _tenantId);
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
}