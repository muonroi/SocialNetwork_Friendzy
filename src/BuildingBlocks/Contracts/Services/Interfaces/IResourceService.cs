namespace Contracts.Services.Interfaces;

public interface IResourceService<T> where T : class
{
    Task UploadResourceAsync(T request, CancellationToken cancellationToken = new());

    Task RemoveResourceAsync(T request, CancellationToken cancellationToken = new());

    Task UpdateResourceAsync(T request, CancellationToken cancellationToken = new());

    Task<T> GetResourceAsync(T request, CancellationToken cancellationToken = new());
}