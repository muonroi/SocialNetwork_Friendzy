using Contracts.DTOs.ResourceDTOs;
using Infrastructure.Helper;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Shared.Services.Resources;
namespace Infrastructure.Services;

public class MinIOResourceService(ILogger logger, IMinioClient minioClient) : IMinIOResourceService
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMinioClient _minioClient = minioClient ?? throw new ArgumentNullException(nameof(minioClient));


    public async Task<string> GetUrlObject(string bucketName, string objectName, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetUrlObject REQUEST --> {JsonConvert.SerializeObject(new { bucketName, objectName })} <--");
        PresignedGetObjectArgs args = new PresignedGetObjectArgs()
                                    .WithBucket(bucketName)
                                    .WithObject(objectName)
                                    .WithExpiry(60);
        string result = await _minioClient.PresignedGetObjectAsync(args);
        _logger.Information($"END: GetUrlObject RESULT --> {JsonConvert.SerializeObject(result.Length)} <-- ");
        return result;
    }

    public async Task<Stream> DownloadResourceStreamAsync(string bucketName, string objectName, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: DownloadResourceStreamAsync REQUEST --> {JsonConvert.SerializeObject(new { bucketName, objectName })} <--");
        Stream result = await _minioClient.DownloadBucketStreamAsync(bucketName, objectName, cancellationToken);
        _logger.Information($"END: DownloadResourceStreamAsync RESULT --> {JsonConvert.SerializeObject(result.Length)} <-- ");
        return result;
    }

    public async Task<ImportObjectResourceDTO?> ImportResourceAsync(string bucketName, MinIOUploadRequest request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: ImportResourceAsync REQUEST --> {JsonConvert.SerializeObject(new { bucketName, request = JsonConvert.SerializeObject(request) })} <--");

        if (request.FormFile is null || request.FormFile.Length is 0)
        {
            return null;
        }

        // Ensure the bucket exists
        bool found = await _minioClient.IsBucketExistAsync(bucketName, cancellationToken);
        if (!found)
        {
            await _minioClient.CreateBucketAsync(bucketName, cancellationToken).ConfigureAwait(false);
        }

        PutObjectResponse statObj = await _minioClient.UploadStreamObjectAsync(bucketName, request, cancellationToken);

        _logger.Information($"END: ImportResourceAsync RESULT --> {JsonConvert.SerializeObject(statObj)} <-- ");
        ImportObjectResourceDTO result = new(await GetUrlObject(bucketName, request.FileName, cancellationToken), statObj.ObjectName);
        return result;
    }

    public async Task<IEnumerable<ImportObjectResourceDTO>?> ImportMultipleResourceAsync(string bucketName, IEnumerable<MinIOUploadRequest> requests, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: ImportMultipleResourceAsync REQUEST --> {JsonConvert.SerializeObject(new { bucketName, request = JsonConvert.SerializeObject(requests) })} <--");

        if (requests.Any(x => x.FormFile is null || x.FormFile.Length == 0))
        {
            return [];
        }

        // Ensure the bucket exists
        bool found = await _minioClient.IsBucketExistAsync(bucketName, cancellationToken);
        if (!found)
        {
            await _minioClient.CreateBucketAsync(bucketName, cancellationToken).ConfigureAwait(false);
        }

        IEnumerable<Task<string>> uploadTasks = requests.Select(async request =>
        {
            PutObjectResponse statObj = await _minioClient.UploadStreamObjectAsync(bucketName, request, cancellationToken);
            _logger.Information($"Uploaded Object: {JsonConvert.SerializeObject(statObj)}");

            return statObj.Size > 0 ? await GetUrlObject(bucketName, request.FileName, cancellationToken) : string.Empty;
        });

        string[] uploadResult = await Task.WhenAll(uploadTasks);

        _logger.Information($"END: ImportMultipleResourceAsync RESULT --> {JsonConvert.SerializeObject(uploadResult)} <-- ");

        IEnumerable<ImportObjectResourceDTO> result = requests.Zip(uploadResult, (request, url) => new ImportObjectResourceDTO(url, request.FileName));

        return result;
    }

    public async Task RemoveResourceAsync(string bucketName, string objectName, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: RemoveResourceAsync REQUEST --> {JsonConvert.SerializeObject(new { bucketName, objectName })} <--");
        await _minioClient.RemoveObjectAsync(bucketName, objectName, cancellationToken);
        _logger.Information($"END: RemoveResourceAsync RESULT --> none <-- ");
    }

}
