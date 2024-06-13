﻿namespace Management.Photo.Application.Commons.Interfaces;

public interface IStoreInfoRepository : IRepositoryBaseAsync<StoreInfoEntity, long>
{
    Task<IEnumerable<StoreInfoDTO>> GetResourceByTypeAsync(long userId, long bucketId, StoreInfoType type, CancellationToken cancellationToken);

    Task<bool> CreateResourceAsync(CreateResourceRequest request, CancellationToken cancellationToken);

    Task<StoreInfoDTO?> GetResourceByIdAsync(long userId, long bucketId, long storeInfoId, CancellationToken cancellationToken);
}