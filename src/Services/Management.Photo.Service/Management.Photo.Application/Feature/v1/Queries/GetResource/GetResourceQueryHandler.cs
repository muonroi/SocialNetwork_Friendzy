namespace Management.Photo.Application.Feature.v1.Queries.GetResource;

public class GetResourceQueryHandler(IStoreInfoRepository storeInfoRepository, IWorkContextAccessor workContext) : IRequestHandler<GetResourceQuery, ApiResult<IEnumerable<StoreInfoDTO>>>
{
    private readonly IStoreInfoRepository _storeInfoRepository = storeInfoRepository ?? throw new ArgumentNullException(nameof(storeInfoRepository));

    private readonly IWorkContextAccessor _workContext = workContext ?? throw new ArgumentNullException(nameof(workContext));

    public async Task<ApiResult<IEnumerable<StoreInfoDTO>>> Handle(GetResourceQuery request, CancellationToken cancellationToken)
    {
        WorkContextInfoDTO workContext = _workContext.WorkContext!;

        IEnumerable<StoreInfoDTO>? storeInfo = await _storeInfoRepository.GetResourceByTypeAsync(workContext.UserId, request.BucketId, request.Type, cancellationToken);

        return storeInfo is null
            ? new ApiErrorResult<IEnumerable<StoreInfoDTO>>("Store info not found", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<IEnumerable<StoreInfoDTO>>(storeInfo);
    }
}