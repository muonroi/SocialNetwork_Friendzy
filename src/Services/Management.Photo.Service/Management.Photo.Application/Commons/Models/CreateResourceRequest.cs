using Shared.Enums;

namespace Management.Photo.Application.Commons.Models
{
    public record CreateResourceRequest(string ObjectUrl, string ObjectName, long UserId, long BucketId, StoreInfoType Type, bool IsMultiple = false);
}
