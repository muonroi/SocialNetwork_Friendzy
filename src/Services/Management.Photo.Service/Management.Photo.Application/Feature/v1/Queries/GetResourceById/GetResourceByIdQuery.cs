using Management.Photo.Application.Commons.Models;
using MediatR;
using Shared.SeedWorks;

namespace Management.Photo.Application.Feature.v1.Queries.GetResourceById;
public class GetResourceByIdQuery : IRequest<ApiResult<StoreInfoDTO>>
{
    public long Id { get; set; }
    public long BucketId { get; set; }

}
