using Contracts.Commons.Interfaces;
using Management.Photo.Application.Commons.Models;
using Management.Photo.Domain.Entities;

namespace Management.Photo.Application.Commons.Interfaces;

public interface IStoreInfoRepository : IRepositoryBaseAsync<StoreInfoEntity, long>
{
    Task<StoreInfoDTO> ImportResource();
}