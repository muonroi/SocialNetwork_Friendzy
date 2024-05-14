using Dapper.Extensions.Caching;

namespace Infrastructure.Extensions;

public static class RedisExtentions
{
    private const string SectionName = RedisConfigs.SectionName;

    public static IServiceCollection AddDapperCaching(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        string key = configuration.GetValue<string>("SecretKey") ?? string.Empty;
        bool enable = configuration.GetValue<bool>($"{SectionName}:{nameof(RedisConfigs.Enable)}");
        if (!enable)
        {
            return services;
        }

        string? password = configuration.GetEx($"{SectionName}:{nameof(RedisConfigs.Password)}", key);
        string? host = configuration.GetEx($"{SectionName}:{nameof(RedisConfigs.Host)}", key);
        string? port = configuration.GetEx($"{SectionName}:{nameof(RedisConfigs.Port)}", key);
        string? keyPrefix = configuration.GetEx($"{SectionName}:{nameof(RedisConfigs.KeyPrefix)}", key);
        int expire = configuration.GetValue<int>($"{SectionName}:{nameof(RedisConfigs.Expire)}");
        bool allMethodsEnableCache = configuration.GetValue<bool>($"{SectionName}:{nameof(RedisConfigs.AllMethodsEnableCache)}");

        if (string.IsNullOrEmpty(password)
            || string.IsNullOrEmpty(host)
            || string.IsNullOrEmpty(port))
        {
            throw new InvalidConfigException(nameof(HttpStatusCode.InternalServerError), $"Invalid {SectionName}");
        }

        RedisClient redisClient = new(new ConnectionStringBuilder
        {
            Host = $"{host}:{port}",
            Password = password,
        });

        _ = services.AddDapperCachingInRedis(new RedisConfiguration
        {
            AllMethodsEnableCache = allMethodsEnableCache,
            Expire = TimeSpan.FromMinutes(expire),
            KeyPrefix = keyPrefix,
        }, redisClient);

        _ = services.AddSingleton<ICacheProvider, RedisCacheProvider>();

        return services;
    }

    //private static IServiceCollection AddDapperCaching(this IServiceCollection service, CacheConfiguration config, RedisClient client)
    //{
    //    ArgumentNullException.ThrowIfNull(config);
    //    ArgumentNullException.ThrowIfNull(client);
    //    service.AddSingleton<ICacheKeyBuilder, DefaultCacheKeyBuilder>();
    //    service.AddSingleton(new CacheConfiguration
    //    {
    //        AllMethodsEnableCache = config.AllMethodsEnableCache,
    //        Expire = config.Expire,
    //        KeyPrefix = config.KeyPrefix
    //    });
    //    service.AddSingleton(client);
    //    service.AddSingleton<global::Dapper.Extensions.Caching.ICacheProvider, RedisCacheProvider>();
    //    service.AddSingleton<IDataSerializer, DataSerializer>();
    //    return service;
    //}

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        RedisConfigs redisConfigs = configuration.GetOptions<RedisConfigs>(SectionName)
            ?? throw new InvalidConfigException(nameof(HttpStatusCode.InternalServerError), $"Invalid {SectionName}");

        string? password = configuration.GetEx($"{SectionName}:{nameof(RedisConfigs.Password)}");
        string? host = configuration.GetEx($"{SectionName}:{nameof(RedisConfigs.Host)}");
        string? port = configuration.GetEx($"{SectionName}:{nameof(RedisConfigs.Port)}");
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
        {
            throw new InvalidConfigException(nameof(HttpStatusCode.InternalServerError), $"Invalid {SectionName}");
        }

        redisConfigs.Password = password;
        redisConfigs.Host = host;
        redisConfigs.Port = port;

        _ = services.AddStackExchangeRedisCache(option =>
        {
            option.InstanceName = redisConfigs.InstanceName ?? string.Empty;
            option.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { { redisConfigs.Host, int.Parse(redisConfigs.Port) } },
                Password = redisConfigs.Password
            };
        });

        return services;
    }

    public static async Task<string?> GetCacheAsync(this IDistributedCache distributedCache, string key, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(distributedCache);
        ArgumentException.ThrowIfNullOrEmpty(key);

        byte[]? cacheValue = await distributedCache.GetAsync(key, token);
        return cacheValue is not null ? Encoding.UTF8.GetString(cacheValue) : default;
    }

    public static async Task<T?> GetCacheAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(distributedCache);
        ArgumentException.ThrowIfNullOrEmpty(key);

        byte[]? cacheValue = await distributedCache.GetAsync(key, token);
        if (cacheValue is not null)
        {
            string valueString = Encoding.UTF8.GetString(cacheValue);
            return System.Text.Json.JsonSerializer.Deserialize<T>(valueString);
        }
        return default;
    }

    public static async Task SetCacheAsync<T>(this IDistributedCache distributedCache, string key, T value)
    {
        ArgumentNullException.ThrowIfNull(distributedCache);
        ArgumentException.ThrowIfNullOrEmpty(key);

        string serializeValue = System.Text.Json.JsonSerializer.Serialize(value);
        byte[] saveValue = Encoding.UTF8.GetBytes(serializeValue);

        await distributedCache.SetAsync(key, saveValue);
    }

    public static async Task RemoveAsync(this IDistributedCache distributedCache, string key, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(distributedCache);
        ArgumentException.ThrowIfNullOrEmpty(key);

        await distributedCache.RemoveAsync(key, token);
    }

    public static async Task RefreshAsync(this IDistributedCache distributedCache, string key, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(distributedCache);
        ArgumentException.ThrowIfNullOrEmpty(key);

        await distributedCache.RefreshAsync(key, token);
    }

    public static async Task<T?> GetOrSetAsync<T>(this IDistributedCache distributedCache
        , string key
        , Func<Task<T>> cacheData
        , CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(distributedCache);

            byte[]? cacheValue = await distributedCache.GetAsync(key, cancellationToken);
            if (cacheValue != null)
            {
                string valueString = Encoding.UTF8.GetString(cacheValue);
                return string.IsNullOrEmpty(valueString) ? default : System.Text.Json.JsonSerializer.Deserialize<T>(valueString);
            }
            else
            {
                T? data = await cacheData();
                string serializeValue = System.Text.Json.JsonSerializer.Serialize(data);
                byte[] saveValue = Encoding.UTF8.GetBytes(serializeValue);

                await distributedCache.SetAsync(key, saveValue, token: cancellationToken);
                return data;
            }
        }
        catch
        {
            return default;
        }
    }
}