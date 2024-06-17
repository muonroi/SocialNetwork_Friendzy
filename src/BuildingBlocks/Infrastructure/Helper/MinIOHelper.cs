namespace Infrastructure.Helper;

public static class MinIOHelper
{
    public static async Task<bool> IsBucketExistAsync(this IMinioClient minioClient, string bucketName, CancellationToken cancellationToken)
    {
        BucketExistsArgs beArgs = new BucketExistsArgs()
            .WithBucket(bucketName);

        return await minioClient.BucketExistsAsync(beArgs, cancellationToken).ConfigureAwait(false);
    }

    public static async Task CreateBucketAsync(this IMinioClient minioClient, string bucketName, CancellationToken cancellationToken)
    {
        MakeBucketArgs mbArgs = new MakeBucketArgs()
                .WithBucket(bucketName);

        await minioClient.MakeBucketAsync(mbArgs, cancellationToken).ConfigureAwait(false);

        SetVersioningArgs svArgs = new SetVersioningArgs()
            .WithBucket(bucketName)
            .WithVersioningEnabled();

        await minioClient.SetVersioningAsync(svArgs, cancellationToken).ConfigureAwait(false);
    }

    public static async Task UploadStreamObjectAsync<T>(this IMinioClient minioClient, string bucketName, T content, CancellationToken cancellationToken) where T : BaseResourceRequest
    {
        using Stream memoryStream = content.FormFile.OpenReadStream();

        memoryStream.Position = 0;

        PutObjectArgs putAnotherObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(content.FileName)
            .WithStreamData(memoryStream)
            .WithObjectSize(memoryStream.Length)
            .WithContentType(content.ContentType);

        await minioClient.PutObjectAsync(putAnotherObjectArgs, cancellationToken);
    }

    public static async Task UploadMultipleStreamObjectsAsync<T>(this IMinioClient minioClient, string bucketName, IEnumerable<T> contents, CancellationToken cancellationToken) where T : BaseResourceRequest
    {
        IEnumerable<Task> tasks = contents.Select(async content =>
        {
            using Stream memoryStream = content.FormFile.OpenReadStream();
            memoryStream.Position = 0;

            PutObjectArgs putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(content.FileName)
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length)
                .WithContentType(content.ContentType);
            await minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);
        });
        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    public static async Task<Stream> DownloadBucketStreamAsync(this IMinioClient minioClient, string bucketName, string objectName, CancellationToken cancellationToken)
    {
        //Ensure the object exists
        await minioClient.GetStatObjectArgs(bucketName, objectName, cancellationToken).ConfigureAwait(false);

        MemoryStream outputStream = new();
        GetObjectArgs getObjectArgs = new GetObjectArgs()
        .WithBucket(bucketName)
        .WithObject(objectName)
        .WithCallbackStream((stream) =>
        {
            stream.CopyTo(outputStream);
        });

        // Ensure object download complete before return the stream
        _ = await minioClient.GetObjectAsync(getObjectArgs, cancellationToken).ConfigureAwait(false);

        // reset strean to 0
        _ = outputStream.Seek(0, SeekOrigin.Begin);

        return outputStream;
    }

    public static async Task RemoveObjectAsync(this IMinioClient minioClient, string bucketName, string objectName, CancellationToken cancellationToken)
    {
        //Ensure the object exists
        await minioClient.GetStatObjectArgs(bucketName, objectName, cancellationToken).ConfigureAwait(false);

        RemoveObjectArgs rmArgs = new RemoveObjectArgs()
                             .WithBucket(objectName)
                             .WithObject(objectName);

        await minioClient.RemoveObjectAsync(rmArgs, cancellationToken).ConfigureAwait(false);
    }

    private static async Task GetStatObjectArgs(this IMinioClient minioClient, string bucketName, string objectName, CancellationToken cancellationToken)
    {
        StatObjectArgs statObjectArgs = new StatObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);
        _ = await minioClient.StatObjectAsync(statObjectArgs, cancellationToken);
    }
}