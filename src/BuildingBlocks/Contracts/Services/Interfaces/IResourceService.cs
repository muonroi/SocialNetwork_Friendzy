using Contracts.DTOs.ResourceDTOs;

namespace Contracts.Services.Interfaces;

public interface IResourceService<T> where T : class
{
    Task<ImportObjectResourceDTO?> ImportResourceAsync(string bucketName, T request, CancellationToken cancellationToken);

    Task<IEnumerable<ImportObjectResourceDTO>?> ImportMultipleResourceAsync(string bucketName, IEnumerable<T> request, CancellationToken cancellationToken);

    Task RemoveResourceAsync(string bucketName, string objectName, CancellationToken cancellationToken);

    Task<Stream> DownloadResourceStreamAsync(string bucketName, string objectName, CancellationToken cancellationToken);
}