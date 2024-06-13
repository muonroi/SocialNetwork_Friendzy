namespace Distance.Service.Infrastructure.Interface;

public interface IDistanceServiceRepository : IRepositoryBaseAsync<DistanceEntity, long>
{
    Task<DistanceResponse> GetDistanceAsync(DistanceRequest request);

    Task<bool> CreateDistanceAsync(DistanceCreateRequest request, CancellationToken cancellationToken);
}