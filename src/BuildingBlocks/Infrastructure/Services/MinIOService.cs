using Shared.Services.Resources;

namespace Infrastructure.Services
{
    internal class MinIOService(ILogger logger, MinIOConfig minIOConfig) : IResourceService
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        private readonly MinIOConfig _minIOConfig = minIOConfig ?? throw new ArgumentNullException(nameof(minIOConfig));

        public Task<MinIOUploadRequest> GetResourceAsync(MinIOUploadRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveResourceAsync(MinIOUploadRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateResourceAsync(MinIOUploadRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UploadResourceAsync(MinIOUploadRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}