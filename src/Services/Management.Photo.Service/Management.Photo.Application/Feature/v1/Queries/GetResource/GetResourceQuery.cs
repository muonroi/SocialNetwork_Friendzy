using Management.Photo.Application.Commons.Models;
using MediatR;
using Shared.Enums;
using Shared.SeedWorks;

namespace Management.Photo.Application.Feature.v1.Queries.GetResource;

public class GetResourceQuery : IRequest<ApiResult<IEnumerable<StoreInfoDTO>>>
{
    public StoreInfoType Type { get; set; }

    public long BucketId { get; set; }

}