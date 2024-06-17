namespace Infrastructure.Services;

public class MinIOResourceService(ILogger logger, IMinioClient minioClient, ISerializeService serializeService) : IMinIOResourceService
{
    private readonly ISerializeService _serializeService = serializeService ?? throw new ArgumentNullException(nameof(serializeService));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMinioClient _minioClient = minioClient ?? throw new ArgumentNullException(nameof(minioClient));

    public async Task<string> GetUrlObject(string bucketName, string objectName, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetUrlObject REQUEST --> {_serializeService.Serialize(new { bucketName, objectName })} <--");
        PresignedGetObjectArgs args = new PresignedGetObjectArgs()
                                    .WithBucket(bucketName)
                                    .WithObject(objectName)
                                    .WithExpiry(604799);
        string result = await _minioClient.PresignedGetObjectAsync(args);
        _logger.Information($"END: GetUrlObject RESULT --> {_serializeService.Serialize(result.Length)} <-- ");
        return result;
    }

    public async Task<Stream> DownloadResourceStreamAsync(string bucketName, string objectName, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: DownloadResourceStreamAsync REQUEST --> {_serializeService.Serialize(new { bucketName, objectName })} <--");
        Stream result = await _minioClient.DownloadBucketStreamAsync(bucketName, objectName, cancellationToken);
        _logger.Information($"END: DownloadResourceStreamAsync RESULT --> {_serializeService.Serialize(result.Length)} <-- ");
        return result;
    }

    public async Task<ImportObjectResourceDTO?> ImportResourceAsync(string bucketName, MinIOUploadRequest request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: ImportResourceAsync REQUEST --> {_serializeService.Serialize(new { bucketName, request = _serializeService.Serialize(request) })} <--");

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

        await _minioClient.UploadStreamObjectAsync(bucketName, request, cancellationToken);

        _logger.Information($"END: ImportResourceAsync RESULT --> {_serializeService.Serialize(request)} <-- ");
        ImportObjectResourceDTO result = new(await GetUrlObject(bucketName, request.FileName, cancellationToken), request.FileName);
        return result;
    }

    public async Task<IEnumerable<ImportObjectResourceDTO>?> ImportMultipleResourceAsync(string bucketName, IEnumerable<MinIOUploadRequest> requests, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: ImportMultipleResourceAsync REQUEST --> {_serializeService.Serialize(new { bucketName, request = _serializeService.Serialize(requests) })} <--");

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
            await _minioClient.UploadStreamObjectAsync(bucketName, request, cancellationToken);
            _logger.Information($"Uploaded Object: {_serializeService.Serialize(request)}");

            return await GetUrlObject(bucketName, request.FileName, cancellationToken);
        });

        string[] uploadResult = await Task.WhenAll(uploadTasks);

        _logger.Information($"END: ImportMultipleResourceAsync RESULT --> {_serializeService.Serialize(uploadResult)} <-- ");

        IEnumerable<ImportObjectResourceDTO> result = requests.Zip(uploadResult, (request, url) => new ImportObjectResourceDTO(url, request.FileName));

        return result;
    }

    public async Task RemoveResourceAsync(string bucketName, string objectName, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: RemoveResourceAsync REQUEST --> {_serializeService.Serialize(new { bucketName, objectName })} <--");
        await _minioClient.RemoveObjectAsync(bucketName, objectName, cancellationToken);
        _logger.Information($"END: RemoveResourceAsync RESULT --> none <-- ");
    }
}