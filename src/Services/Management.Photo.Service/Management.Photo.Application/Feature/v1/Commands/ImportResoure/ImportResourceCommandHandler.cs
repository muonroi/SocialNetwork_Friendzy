using Contracts.Commons.Interfaces;
using Contracts.DTOs.ResourceDTOs;
using Contracts.Services;
using Management.Photo.Application.Commons.Interfaces;
using Management.Photo.Application.Commons.Models;
using Management.Photo.Application.Commons.Requests;
using MediatR;
using Shared.DTOs;
using Shared.SeedWorks;
using Shared.Services.Resources;
using System.Net;

namespace Management.Photo.Application.Feature.v1.Commands.ImportResoure;

public class ImportResourceCommandHandler(
    IStoreInfoRepository storeInfoRepository,
    IWorkContextAccessor workContext,
    IMinIOResourceService resourceService,
    IBucketRepository bucketRepository) : IRequestHandler<ImportResourceCommand, ApiResult<ImportResourceCommandResponse>>
{
    private readonly IMinIOResourceService _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
    private readonly IStoreInfoRepository _storeInfoRepository = storeInfoRepository ?? throw new ArgumentNullException(nameof(storeInfoRepository));
    private readonly IWorkContextAccessor _workContext = workContext ?? throw new ArgumentNullException(nameof(workContext));
    private readonly IBucketRepository _bucketRepository = bucketRepository ?? throw new ArgumentNullException(nameof(bucketRepository));

    public async Task<ApiResult<ImportResourceCommandResponse>> Handle(ImportResourceCommand request, CancellationToken cancellationToken)
    {
        WorkContextInfoDTO workContext = _workContext.WorkContext!;

        //Get bucket info
        BucketDto? getBucketResult = await _bucketRepository.GetBucketByIdAsync((int)request.Type, cancellationToken);
        if (getBucketResult is null)
        {
            return new ApiErrorResult<ImportResourceCommandResponse>("Bucket not found", (int)HttpStatusCode.NotFound);
        }

        // Upload the file to the MinIO server
        MinIOUploadRequest uploadRequest = new()
        {
            FormFile = request.File,
            Type = request.Type,
        };
        ImportObjectResourceDTO? resourceUrl = await _resourceService.ImportResourceAsync(getBucketResult.BucketName, uploadRequest, cancellationToken);
        if (resourceUrl is null)
        {
            return new ApiErrorResult<ImportResourceCommandResponse>("Resource not found", (int)HttpStatusCode.NotFound);
        }
        // Create resource
        _ = await _storeInfoRepository.CreateResourceAsync(
             new CreateResourceRequest(
                 resourceUrl.ObjectUrl,
                 resourceUrl.ObjectName,
                 workContext.UserId,
                 getBucketResult.Id,
                 request.Type), cancellationToken);

        ImportResourceCommandResponse result = new()
        {
            StoreName = resourceUrl.ObjectName,
            StoreInfoType = request.Type,
            StoreDescription = $"{workContext.UserId}_{resourceUrl.ObjectName}_{getBucketResult.BucketName}",
            StoreUrl = resourceUrl.ObjectUrl,
            UserId = workContext.UserId
        };
        return new ApiSuccessResult<ImportResourceCommandResponse>(result);
    }
}
