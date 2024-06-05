namespace Management.Photo.Application.Feature.v1.Queries.GetBuckets;

public record GetBucketQuery : IRequest<ApiResult<IEnumerable<BucketDto>>>
{
}