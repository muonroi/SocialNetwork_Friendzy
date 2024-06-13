namespace Management.Photo.Infrastructure.Query;

public class BucketRepository(StoreInfoDbContext dbContext, IUnitOfWork<StoreInfoDbContext> unitOfWork, ILogger logger, IDapper dapper, IWorkContextAccessor workContextAccessor, ISerializeService serializeService) : RepositoryBaseAsync<BucketEntity, long, StoreInfoDbContext>(dbContext, unitOfWork, workContextAccessor), IBucketRepository
{
    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<BucketDto?> GetBucketByIdAsync(long bucketId, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetBucketByIdAsync REQUEST --> {_serializeService.Serialize(new { bucketId })} <--");
        BucketDto? result = await _dapper.QueryFirstOrDefaultAsync<BucketDto>(CustomSqlQuery.GetBucketById, new { id = bucketId }, cancellationToken: cancellationToken);
        if (result is null)
        {
            _logger.Information($"END: GetBucketByIdAsync RESULT --> {_serializeService.Serialize(result)} <-- ");
            return null;
        }
        _logger.Information($"END: GetBucketByIdAsync RESULT --> {_serializeService.Serialize(result)} <-- ");
        return result;
    }

    public async Task<IEnumerable<BucketDto>?> GetBucketsAsync(CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetBucketsAsync REQUEST --> none <--");
        IEnumerable<BucketDto>? result = await _dapper.QueryAsync<BucketDto>(CustomSqlQuery.GetBuckets, cancellationToken: cancellationToken);
        if (result is null)
        {
            _logger.Information($"END: GetBucketsAsync RESULT --> {_serializeService.Serialize(result)} <-- ");
            return null;
        }
        _logger.Information($"END: GetBucketsAsync RESULT --> {_serializeService.Serialize(result)} <-- ");
        return result;
    }
}