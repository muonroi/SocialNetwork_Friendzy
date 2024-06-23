namespace Shared.Configurations
{
    public record DatabaseConfigs
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}