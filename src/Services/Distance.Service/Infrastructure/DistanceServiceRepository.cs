namespace Distance.Service.Infrastructure;

public class DistanceServiceRepository(DistanceDbContext distanceDbContext, IUnitOfWork<DistanceDbContext> unitOfWork, ILogger logger, IDapper dapper, IWorkContextAccessor workContextAccessor, ISerializeService serializeService) : RepositoryBaseAsync<DistanceEntity, long, DistanceDbContext>(distanceDbContext, unitOfWork, workContextAccessor), IDistanceServiceRepository
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IDapper _dapper = dapper;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<DistanceResponse> GetDistanceAsync(DistanceRequest request)
    {
        //remove space and convert to lower case
        request.Country = request.Country.Replace(" ", string.Empty).ToLower();
        _logger.Information($"BEGIN: GetDistanceAsync REQUEST --> {_serializeService.Serialize(request)} <--");
        DapperCommand command = new()
        {
            CommandText = CustomSqlQuery.GetDistanceByCountry,
            Parameters = new
            {
                request.Country,
                request.PageSize,
                request.PageIndex,
                request.UserIds
            },
        };
        PageResult<DistanceEntity> dataDistanceResult = await _dapper.QueryPageAsync<DistanceEntity>(command: command, CustomSqlQuery.GetDistanceCountInfo, request.PageIndex, request.PageSize);
        if (dataDistanceResult.Result.Count == 0)
        {
            return new DistanceResponse();
        }

        DistanceResponse result = new()
        {
            Items = dataDistanceResult.Result.Select(x => new DistanceResponseItem
            {
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Country = x.Country,
                UserId = x.UserId,
            }),
            TotalItems = dataDistanceResult.TotalCount
        };

        _logger.Information($"END: GetDistanceAsync RESULT --> {_serializeService.Serialize(result)} <-- ");

        return result;
    }

    public async Task<bool> CreateDistanceAsync(DistanceCreateRequest request, CancellationToken cancellationToken)
    {
        //remove space and convert to lower case
        request.Country = request.Country.Replace(" ", string.Empty).ToLower();
        _logger.Information($"BEGIN: CreateDistanceAsync REQUEST --> {_serializeService.Serialize(request)} <--");

        _ = await CreateAsync(new DistanceEntity
        {
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Country = request.Country,
            UserId = request.UserId
        }, cancellationToken);

        int result = await SaveChangesAsync();

        _logger.Information($"END: CreateDistanceAsync RESULT --> {_serializeService.Serialize(result)} <-- ");

        return result > 0;
    }
}