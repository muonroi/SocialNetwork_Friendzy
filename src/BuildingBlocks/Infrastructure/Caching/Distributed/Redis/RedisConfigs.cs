namespace Infrastructure.Caching.Distributed.Redis
{
    public class RedisConfigs
    {
        public const string SectionName = "RedisConfigs";
        public string Host { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string InstanceName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool AllowAdmin { get; set; }
        public bool Enable { get; set; }
        public bool AllMethodsEnableCache { get; set; }
        public string KeyPrefix { get; set; } = string.Empty;
        public int Expire { get; set; }
        public bool AbortOnConnectFail { get; set; }
    }
}