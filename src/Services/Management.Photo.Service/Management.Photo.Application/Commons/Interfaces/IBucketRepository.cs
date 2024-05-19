using Management.Photo.Application.Commons.Models;

namespace Management.Photo.Application.Commons.Interfaces
{
    public interface IBucketRepository
    {
        Task<IEnumerable<BucketDto>?> GetBucketsAsync(CancellationToken cancellationToken);
        Task<BucketDto?> GetBucketByIdAsync(long bucketId, CancellationToken cancellationToken);
    }
}
