using Shared.Enums;

namespace Shared.Services.Resources
{
    public class MinIOUploadRequest
    {
        public string StoreName { get; set; } = string.Empty;

        public string StoreDescription { get; set; } = string.Empty;

        public string StoreUrl { get; set; } = string.Empty;

        public StoreInfoType Type { get; set; }
    }
}