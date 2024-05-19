namespace Shared.Services.Resources
{
    public record MinIODownloadResourceRequest
    {
        public string FileName { get; set; } = string.Empty;
    }
}
