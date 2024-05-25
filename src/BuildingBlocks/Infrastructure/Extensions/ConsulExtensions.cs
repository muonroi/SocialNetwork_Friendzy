namespace Infrastructure.Extensions;

public static class ConsulExtensions
{
    private static readonly string defaultProtocol = "http";

    public static async Task<string> GetUriOnConsulAsync(this IConsulClient consulClient, string serviceName)
    {
        ArgumentNullException.ThrowIfNull(consulClient);

        serviceName = serviceName.Trim();
        if (string.IsNullOrEmpty(serviceName))
        {
            throw new ArgumentNullException(nameof(serviceName));
        }

        Dictionary<string, AgentService>.ValueCollection services = (await consulClient.Agent.Services()).Response.Values;
        List<AgentService> filteredServices = services
              .Where(x => x.Service == serviceName)
              .ToList();
        if (!filteredServices.Any())
        {
            throw new ConsulServiceNotFoundException($"Service '{serviceName}' not found in Consul.");
        }
        List<AgentService> shareTenantServices = [];
        foreach (AgentService? service in filteredServices)
        {
            if (!service.Meta.Any())
            {
                shareTenantServices.Add(service);
            }
        }
        if (shareTenantServices.Count == 0)
        {
            shareTenantServices = filteredServices;
        }

        Random random = new();
        //var protocol = selectedService.Meta.ContainsKey("protocol")
        //    ? selectedService.Meta["protocol"]
        //    : "http";

        return $"{defaultProtocol}";
    }

    public static IServiceCollection AddConsul(this IServiceCollection services, ConsulConfigs consulConfigs, IWebHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            _ = services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                string address = consulConfigs.ConsulAddress!;
                consulConfig.Address = new Uri(address);
            }));
        }
        return services;
    }

    public static IApplicationBuilder UseConsul(this IApplicationBuilder app, ConsulConfigs consulSettings, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return app;
        }
        IConsulClient consulClient = app.ApplicationServices
                           .GetRequiredService<IConsulClient>();
        IHostApplicationLifetime lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        string? address = $"{consulSettings.ServiceAddress}:{consulSettings.ServicePort}";

        if (string.IsNullOrWhiteSpace(address))
        {
            FeatureCollection? features = app.Properties["server.Features"] as FeatureCollection;
            IServerAddressesFeature? addresses = features?.Get<IServerAddressesFeature>();
            address = addresses?.Addresses.First() ?? string.Empty;

            Console.WriteLine($"Could not find service address in config. " +
                $"Using '{address}'");
        }

        Uri uri = new(address);
        string serviceName = consulSettings.ServiceName ?? AppDomain.CurrentDomain.FriendlyName.Trim().Trim('_');
        AgentServiceRegistration registration = new()
        {
            ID = $"{serviceName.ToLowerInvariant()}-{consulSettings.Id ?? Guid.NewGuid().ToString()}",
            Name = serviceName,
            Address = uri.Host,
            Port = uri.Port,
            Meta = consulSettings.ServiceMetadata ?? []
        };

        consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        consulClient.Agent.ServiceRegister(registration).Wait();

        _ = lifetime.ApplicationStopping.Register(() =>
        {
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });

        return app;
    }
}