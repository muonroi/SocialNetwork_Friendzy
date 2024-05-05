using Contracts.Commons.Interfaces;
using Distance.Service.Domains;
using Distance.Service.Models;

namespace Distance.Service.Infrastructure.Interface;

public interface IDistanceServiceRepository : IRepositoryBaseAsync<DistanceEntity, long>
{
    Task<DistanceResponse> GetDistanceAsync(DistanceRequest request);
}