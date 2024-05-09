using Contracts.Services.Interfaces;
using Shared.Services.Resources;

namespace Contracts.Services
{
    public interface IResourceService : IResourceService<MinIOUploadRequest>
    { }
}