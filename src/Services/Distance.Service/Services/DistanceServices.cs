namespace Distance.Service.Services;

public class DistanceServices(ILogger<DistanceServices> logger, IDistanceServiceRepository distanceServiceRepository, ISerializeService serializeService) : DistanceServiceBase
{
    private readonly ILogger<DistanceServices> _logger = logger;

    private readonly ISerializeService _serializeService = serializeService;

    private readonly IDistanceServiceRepository _distanceServiceRepository = distanceServiceRepository;

    public override async Task<GetDistanceInfoListReply> GetDistanceInfoList(GetDistanceInfoListRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"BEGIN: GetDistanceInfoList REQUEST --> {_serializeService.Serialize(request)} <--");

        DistanceResponse distances = await _distanceServiceRepository.GetDistanceAsync(new DistanceRequest
        {
            Country = request.Country,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            UserIds = request.FriendIds
        });
        if (distances.TotalItems == 0)
        {
            return new GetDistanceInfoListReply();
        }
        GetDistanceInfoListReply result = new()
        {
            DistanceInfoList = { distances.Items!.Select(x => new GetDistanceInfoDetail
            {
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Country = x.Country,
                UserId = x.UserId
            })},
            TotalItems = distances.TotalItems
        };

        _logger.LogInformation(message: $"END: GetDistanceInfoList RESULT --> {_serializeService.Serialize(result)} <--");
        return result;
    }

    public override async Task<CreateDistanceInfoReply> CreateDistanceInfo(CreateDistanceInfoRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"BEGIN: CreateDistanceInfo REQUEST --> {_serializeService.Serialize(request)} <--");
        bool distanceCreated = await _distanceServiceRepository.CreateDistanceAsync(new DistanceCreateRequest
        {
            UserId = request.UserId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Country = request.Country
        }, context.CancellationToken);

        CreateDistanceInfoReply result = new()
        {
            IsSuccess = distanceCreated
        };

        _logger.LogInformation(message: $"END: CreateDistanceInfo RESULT --> {_serializeService.Serialize(result)} <--");
        return result;
    }
}