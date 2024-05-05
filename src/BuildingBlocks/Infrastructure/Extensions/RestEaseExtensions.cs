namespace Infrastructure.Extensions;

public static class RestEaseExtensions
{
    public static IHttpClientBuilder RegisterServiceForwarder<T>(this IServiceCollection services,
     string serviceName, string? baseUrl = null)
           where T : class
    {
        _ = services.ConfigureForwarder<T>(serviceName);
        return services.ConfigureRestEastClient(serviceName, baseUrl);
    }

    private static IHttpClientBuilder ConfigureRestEastClient(this IServiceCollection services,
          string serviceName, string? baseUrl = null)
    {
        return services.AddHttpClient(serviceName, (serviceProvider, client) =>
        {
            string _ContentType = "application/json";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
            client.BaseAddress = !string.IsNullOrEmpty(baseUrl) ? new Uri(baseUrl) : new Uri("http://localhost:5000");
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler { MaxConnectionsPerServer = 300, AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate, };
        });
    }

    private static IServiceCollection ConfigureForwarder<T>(this IServiceCollection services, string clientName) where T : class
    {
        _ = services.AddSingleton(c =>
        {
            IHttpClientFactory? httpClientFactory = c.GetService<IHttpClientFactory>();
            ArgumentNullException.ThrowIfNull(httpClientFactory);

            HttpClient client = httpClientFactory.CreateClient(clientName);

            return new RestClient(client)
            {
                RequestQueryParamSerializer = new QueryParamSerializer()
            }.For<T>();
        });
        return services;
    }
}