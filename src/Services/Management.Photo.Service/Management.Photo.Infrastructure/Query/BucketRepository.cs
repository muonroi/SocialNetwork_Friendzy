using Contracts.Commons.Interfaces;
using Dapper.Extensions;
using Infrastructure.Commons;
using Management.Photo.Application.Commons.Interfaces;
using Management.Photo.Application.Commons.Models;
using Management.Photo.Domain.Entities;
using Management.Photo.Infrastructure.Persistences;
using Management.Photo.Infrastructure.Persistences.Query;
using Newtonsoft.Json;
using Serilog;

namespace Management.Photo.Infrastructure.Query
{
    public class BucketRepository(StoreInfoDbContext dbContext, IUnitOfWork<StoreInfoDbContext> unitOfWork, ILogger logger, IDapper dapper) : RepositoryBaseAsync<BucketEntity, long, StoreInfoDbContext>(dbContext, unitOfWork), IBucketRepository
    {
        private readonly ILogger _logger = logger;

        private readonly IDapper _dapper = dapper;

        public async Task<BucketDto?> GetBucketByIdAsync(long bucketId, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: GetBucketByIdAsync");
            BucketDto? result = await _dapper.QueryFirstOrDefaultAsync<BucketDto>(CustomSqlQuery.GetBucketById, new { id = bucketId }, cancellationToken: cancellationToken);
            if (result is null)
            {
                _logger.Information($"END: GetBucketByIdAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
                return null;
            }
            _logger.Information($"END: GetBucketByIdAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
            return result;
        }
        public async Task<IEnumerable<BucketDto>?> GetBucketsAsync(CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: GetBucketsAsync");
            IEnumerable<BucketDto>? result = await _dapper.QueryAsync<BucketDto>(CustomSqlQuery.GetBuckets, cancellationToken: cancellationToken);
            if (result is null)
            {
                _logger.Information($"END: GetBucketsAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
                return null;
            }
            _logger.Information($"END: GetBucketsAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
            return result;
        }
    }
}
