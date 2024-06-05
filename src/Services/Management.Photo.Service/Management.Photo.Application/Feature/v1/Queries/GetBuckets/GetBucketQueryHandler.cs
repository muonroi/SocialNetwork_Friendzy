namespace Management.Photo.Application.Feature.v1.Queries.GetBuckets;

public class GetBucketQueryHandler(IBucketRepository bucketRepository) : IRequestHandler<GetBucketQuery, ApiResult<IEnumerable<BucketDto>>>

{
    private readonly IBucketRepository _bucketRepository = bucketRepository ?? throw new ArgumentNullException(nameof(bucketRepository));

    public async Task<ApiResult<IEnumerable<BucketDto>>> Handle(GetBucketQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<BucketDto>? bucketInfo = await _bucketRepository.GetBucketsAsync(cancellationToken);
        return bucketInfo is null
            ? new ApiErrorResult<IEnumerable<BucketDto>>("Bucket info not found", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<IEnumerable<BucketDto>>(bucketInfo);
    }
}