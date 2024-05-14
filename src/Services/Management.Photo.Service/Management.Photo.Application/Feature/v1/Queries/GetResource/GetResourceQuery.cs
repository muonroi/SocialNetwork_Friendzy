using MediatR;
using Shared.Enums;
using Shared.SeedWorks;

namespace Management.Photo.Application.Feature.v1.Queries.GetResource;

public class GetResourceQuery : IRequest<ApiResult<GetResourceQueryResponse>>
{
    public StoreInfoType Type { get; set; }
}