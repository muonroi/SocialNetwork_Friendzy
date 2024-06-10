namespace Management.Photo.Application.Feature.v1.Commands.ImportMultipleResource;

public class ImportMultipleResourceCommandHandler(
IStoreInfoRepository storeInfoRepository,
IWorkContextAccessor workContext,
IMinIOResourceService resourceService,
IBucketRepository bucketRepository) : IRequestHandler<ImportMultipleResourceCommand, ApiResult<IEnumerable<ImportMultipleResourceCommandResponse>>>
{
    private readonly IMinIOResourceService _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
    private readonly IStoreInfoRepository _storeInfoRepository = storeInfoRepository ?? throw new ArgumentNullException(nameof(storeInfoRepository));
    private readonly IWorkContextAccessor _workContext = workContext ?? throw new ArgumentNullException(nameof(workContext));
    private readonly IBucketRepository _bucketRepository = bucketRepository ?? throw new ArgumentNullException(nameof(bucketRepository));

    public async Task<ApiResult<IEnumerable<ImportMultipleResourceCommandResponse>>> Handle(ImportMultipleResourceCommand request, CancellationToken cancellationToken)
    {
        WorkContextInfoDTO workContext = _workContext.WorkContext!;

        //Get bucket info
        BucketDto? getBucketResult = await _bucketRepository.GetBucketByIdAsync((int)request.Type, cancellationToken);
        if (getBucketResult is null)
        {
            return new ApiErrorResult<IEnumerable<ImportMultipleResourceCommandResponse>>("Bucket not found", (int)HttpStatusCode.NotFound);
        }
        // Upload the file to the MinIO server
        IEnumerable<MinIOUploadRequest> uploadRequests = request.Files.Select(x => new MinIOUploadRequest
        {
            FormFile = x,
            Type = request.Type,
        });

        IEnumerable<ImportObjectResourceDTO>? resourceUrl = await _resourceService.ImportMultipleResourceAsync(getBucketResult.BucketName, uploadRequests, cancellationToken);

        if (resourceUrl is null || !resourceUrl.Any())
        {
            return new ApiErrorResult<IEnumerable<ImportMultipleResourceCommandResponse>>("Resource not found", (int)HttpStatusCode.NotFound);
        }

        // Create resource
        IEnumerable<Task> createTask = resourceUrl.Select(async x =>
        {
            _ = await _storeInfoRepository.CreateResourceAsync(
             new CreateResourceRequest(
                 x.ObjectUrl,
                 x.ObjectName,
                 workContext.UserId,
                 getBucketResult.Id,
                 request.Type,
                 true), cancellationToken);
        });

        await Task.WhenAll(createTask);

        IEnumerable<ImportMultipleResourceCommandResponse> result = resourceUrl.Select(x => new ImportMultipleResourceCommandResponse
        {
            StoreName = x.ObjectName,
            StoreInfoType = request.Type,
            StoreDescription = $"{workContext.UserId}_{x.ObjectName}_{getBucketResult.BucketName}",
            StoreUrl = x.ObjectUrl,
            UserId = workContext.UserId
        });
        return new ApiSuccessResult<IEnumerable<ImportMultipleResourceCommandResponse>>(result);
    }
}