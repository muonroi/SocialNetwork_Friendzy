using Contracts.Commons.Interfaces;
using Management.Photo.Application.Commons.Models;
using Management.Photo.Domain.Entities;
using Shared.Enums;

namespace Management.Photo.Application.Commons.Interfaces;

public interface IStoreInfoRepository : IRepositoryBaseAsync<StoreInfoEntity, long>
{
    Task<List<StoreInfoDTO>> GetResourceByName(long userId, StoreInfoType type);
}