using Contracts.Commons.Interfaces;
using Management.Photo.Application.Commons.Models;
using Management.Photo.Application.Commons.Requests;
using Management.Photo.Domain.Entities;
using Shared.Enums;

namespace Management.Photo.Application.Commons.Interfaces;

public interface IStoreInfoRepository : IRepositoryBaseAsync<StoreInfoEntity, long>
{
    Task<IEnumerable<StoreInfoDTO>> GetResourceByTypeAsync(long userId, long bucketId, StoreInfoType type, CancellationToken cancellationToken);
    Task<bool> CreateResourceAsync(CreateResourceRequest request, CancellationToken cancellationToken);
    Task<StoreInfoDTO?> GetResourceByIdAsync(long userId, long bucketId, long storyInfoId, CancellationToken cancellationToken);
}