using Contracts.Domains;

namespace Management.Photo.Domain.Entities
{
    public class BucketEntity : EntityAuditBase<long>
    {
        public string BucketName { get; set; } = string.Empty;
        public string BucketDescription { get; set; } = string.Empty;
        public required IEnumerable<StoreInfoEntity> StoreInfos { get; set; }
    }
}
