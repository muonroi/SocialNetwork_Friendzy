using Distance.Service.Persistence;

namespace Distance.Service.Infrastructure;

public class DistanceServiceRepository(DistanceDbContext distanceDbContext, IUnitOfWork<DistanceDbContext> unitOfWork, ILogger logger, IDapper dapper) : RepositoryBaseAsync<DistanceEntity, long, DistanceDbContext>(distanceDbContext, unitOfWork), IDistanceServiceRepository
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IDapper _dapper = dapper;

    public async Task<DistanceResponse> GetDistanceAsync(DistanceRequest request)
    {
        //remove space and convert to lower case
        request.Country = request.Country.Replace(" ", string.Empty).ToLower();
        _logger.Information($"BEGIN: GetDistanceAsync REQUEST --> {JsonConvert.SerializeObject(request)} <--");
        DapperCommand command = new()
        {
            CommandText = CustomSqlQuery.GetDistanceByCountry,
            Parameters = new
            {
                request.Country,
                pageSize = request.PageSize,
                pageIndex = request.PageIndex
            },
            CommandType = CommandType.StoredProcedure,
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

        _logger.Information($"END: GetDistanceAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");

        return result;
    }
}