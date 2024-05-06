using Contracts.Commons.Interfaces;
using Management.Photo.Domain.Entities;

namespace Management.Photo.Application.Commons.Interfaces;

public interface IStoreInfoRepository : IRepositoryBaseAsync<StoreInfoEntity, long>
{
}