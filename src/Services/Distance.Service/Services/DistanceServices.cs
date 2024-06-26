﻿namespace Distance.Service.Services;

public class DistanceServices(ILogger<DistanceServices> logger, IDistanceServiceRepository distanceServiceRepository) : DistanceServiceBase
{
    private readonly ILogger<DistanceServices> _logger = logger;

    private readonly IDistanceServiceRepository _distanceServiceRepository = distanceServiceRepository;

    public override async Task<GetDistanceInfoListReply> GetDistanceInfoList(GetDistanceInfoListRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"BEGIN: GetDistanceInfoList REQUEST --> {JsonConvert.SerializeObject(request)} <--");
        DistanceResponse distances = await _distanceServiceRepository.GetDistanceAsync(new DistanceRequest
        {
            Country = request.Country,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        });

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

        _logger.LogInformation(message: $"END: GetDistanceInfoList RESULT --> {JsonConvert.SerializeObject(result)} <--");
        return result;
    }
}