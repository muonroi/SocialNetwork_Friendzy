namespace Infrastructure.Commons;

public class RestEaseServiceDiscoveryMessageHandler(
 Func<HttpRequestHeaders, Task<Dictionary<string, string>>> callbackApi) : DelegatingHandler
{
    private const string _methodKey = "Method-Key";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Uri? current = request.RequestUri;
        HttpRequestHeaders headers = request.Headers;

        try
        {
            string? methodValue = headers.GetValues(_methodKey).FirstOrDefault();
            if (!string.IsNullOrEmpty(methodValue))
            {
                Dictionary<string, string> options = await callbackApi(request.Headers);
                if (options != null && options.TryGetValue(methodValue.Trim(), out string? url))
                {
                    ArgumentException.ThrowIfNullOrEmpty(url);
                    Uri uri = new(url);
                    string pathAndQuery = (current?.PathAndQuery.StartsWith("/?") == true
                                        ? current?.PathAndQuery.TrimStart('/')
                                        : current?.PathAndQuery) ?? string.Empty;

                    request.RequestUri = new Uri(uri, uri.PathAndQuery + pathAndQuery);
                }
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