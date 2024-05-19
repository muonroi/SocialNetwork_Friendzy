using Contracts.Commons.Interfaces;
using Management.Photo.Application.Commons.Interfaces;
using Management.Photo.Application.Commons.Models;
using MediatR;
using Shared.DTOs;
using Shared.SeedWorks;
using System.Net;

namespace Management.Photo.Application.Feature.v1.Queries.GetResourceById;
public class GetResourceByIdQueryHandler(IStoreInfoRepository storeInfoRepository, IWorkContextAccessor workContext) : IRequestHandler<GetResourceByIdQuery, ApiResult<StoreInfoDTO>>
{
    private readonly IStoreInfoRepository _storeInfoRepository = storeInfoRepository ?? throw new ArgumentNullException(nameof(storeInfoRepository));

    private readonly IWorkContextAccessor _workContext = workContext ?? throw new ArgumentNullException(nameof(workContext));

    public async Task<ApiResult<StoreInfoDTO>> Handle(GetResourceByIdQuery request, CancellationToken cancellationToken)
    {
        WorkContextInfoDTO workContext = _workContext.WorkContext!;

        StoreInfoDTO? storeInfo = await _storeInfoRepository.GetResourceByIdAsync(workContext.UserId, request.BucketId, request.Id, cancellationToken);
        return storeInfo is null
            ? new ApiErrorResult<StoreInfoDTO>("Store info not found", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<StoreInfoDTO>(storeInfo);
    }
}
