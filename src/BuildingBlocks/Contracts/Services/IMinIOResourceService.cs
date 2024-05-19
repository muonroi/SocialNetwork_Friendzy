using Contracts.Services.Interfaces;
using Shared.Services.Resources;

namespace Contracts.Services;

public interface IMinIOResourceService : IResourceService<MinIOUploadRequest>
{
    Task<string> GetUrlObject(string bucketName, string objectName, CancellationToken cancellationToken);
}