namespace Infrastructure.Configurations
{
    public class ConsulConfigs
    {
        public const string SectionName = "ConsulConfigs";
        public string? Id { get; set; }
        public string? ServiceName { get; set; }
        public string? ConsulAddress { get; set; }
        public string? ServiceAddress { get; set; }
        public string? ServicePort { get; set; }
        public Dictionary<string, string>? ServiceMetadata { get; set; }
    }
}