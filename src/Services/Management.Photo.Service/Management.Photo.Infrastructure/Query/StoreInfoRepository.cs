namespace Management.Photo.Infrastructure.Query;

public class StoreInfoRepository(StoreInfoDbContext dbContext, IDbContextFactory<StoreInfoDbContext> dbContextFactory, IUnitOfWork<StoreInfoDbContext> unitOfWork, ILogger logger, IDapper dapper, IBucketRepository bucketRepository, IWorkContextAccessor workContextAccessor, ISerializeService serializeService) : RepositoryBaseAsync<StoreInfoEntity, long, StoreInfoDbContext>(dbContext, unitOfWork, workContextAccessor), IStoreInfoRepository
{

    private readonly ISerializeService _serializeService = serializeService;

    private readonly IDbContextFactory<StoreInfoDbContext> _dbContextFactory = dbContextFactory;

    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    private readonly IBucketRepository _bucketRepository = bucketRepository;

    public async Task<bool> CreateResourceAsync(CreateResourceRequest request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: CreateResourceAsync REQUEST --> {_serializeService.Serialize(request)} <-- REQUEST");
        StoreInfoDbContext? storeInfoDbContext = request.IsMultiple ? _dbContextFactory.CreateDbContext() : null;
        BucketDto? bucketInfo = await _bucketRepository.GetBucketByIdAsync(request.BucketId, cancellationToken);
        if (bucketInfo is null)
        {
            _logger.Information($"END: CreateResourceAsync RESULT --> {_serializeService.Serialize(false)} <-- ");
            return false;
        }
        if (storeInfoDbContext is not null)
        {
            _ = await storeInfoDbContext.StoreInfoEntities.AddAsync(new StoreInfoEntity
            {
                StoreName = request.ObjectName,
                StoreDescription = $"{request.UserId}_{request.ObjectName}_{bucketInfo.BucketName}",
                StoreUrl = request.ObjectUrl.StringToBase64(),
                UserId = request.UserId,
                BucketId = bucketInfo.Id,
                StoreInfoType = request.Type
            }, cancellationToken);
            _ = await storeInfoDbContext.SaveChangesAsync(cancellationToken);
            return true; // edit after
        }
        _ = await CreateAsync(new StoreInfoEntity
        {
            StoreName = request.ObjectName,
            StoreDescription = $"{request.UserId}_{request.ObjectName}_{bucketInfo.BucketName}",
            StoreUrl = request.ObjectUrl.StringToBase64(),
            UserId = request.UserId,
            BucketId = bucketInfo.Id,
            StoreInfoType = request.Type
        }, cancellationToken);

        long result = await SaveChangesAsync();

        _logger.Information($"END: CreateResourceAsync RESULT --> {_serializeService.Serialize(result)} <-- ");

        return result > 0;
    }

    public async Task<StoreInfoDTO?> GetResourceByIdAsync(long userId, long bucketId, long storyInfoId, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetResourceById REQUEST --> {_serializeService.Serialize(new
        {
            userId,
            bucketId,
            storyInfoId
        })} <--");
        CommandDefinition command = new(CustomSqlQuery.GetImageById, new
        {
            userId,
            bucketId,
            storyInfoId
        }, commandType: CommandType.Text);

        StoreInfoDTO? result = await _dapper.QueryFirstOrDefaultAsync<StoreInfoDTO>(command);
        result.StoreUrl = result.StoreUrl.Base64ToString();
        if (result is null)
        {
            _logger.Information($"END: GetResourceById RESULT --> {_serializeService.Serialize(result)} <-- ");
            return null;
        }
        _logger.Information($"END: GetResourceById RESULT --> {_serializeService.Serialize(result)} <-- ");
        return result;
    }

    public async Task<IEnumerable<StoreInfoDTO>> GetResourceByTypeAsync(long userId, long bucketId, StoreInfoType type, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetResourceByType REQUEST --> {_serializeService.Serialize(new
        {
            userId,
            bucketId,
            typpe = nameof(type)
        })} <--");
        CommandDefinition command = new(CustomSqlQuery.GetImageByType, new
        {
            userId,
            bucketId,
            storeInfoType = type
        }, commandType: CommandType.Text);

        List<StoreInfoDTO> result = await _dapper.QueryAsync<StoreInfoDTO>(command);

        if (result.Count == 0)
        {
            _logger.Information($"END: GetResourceByType RESULT --> {_serializeService.Serialize(result)} <-- ");
            return [];
        }
        _logger.Information($"END: GetResourceByType RESULT --> {_serializeService.Serialize(result)} <-- ");
        return result;
    }
}