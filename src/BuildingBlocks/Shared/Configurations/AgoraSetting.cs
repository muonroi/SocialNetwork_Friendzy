namespace Shared.Configurations
{
    public record AgoraSetting
    {
        public string? AppId { get; set; }
        public string? AppCertificate { get; set; }
    }
}