using Dapper.Extensions;
using Shared.Enums;

namespace Shared.Services.Resources
{
    public abstract record BaseResourceRequest
    {
        public required IFormFile FormFile { get; set; }

        public string FileName => $"{DateTime.UtcNow.ToTimestamp()}_{Guid.NewGuid()}_{FormFile.FileName}";

        public string ContentType => FormFile.ContentType;

        public StoreInfoType Type { get; set; }
    }
}
