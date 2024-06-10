using Contracts.Commons.Interfaces;

namespace SearchPartners.Service.Services;

public class SearchPartnersService(ILogger logger, ISerializeService serializeService) : SearchPartnerServiceBase
{
    private readonly ILogger _logger = logger;

    private readonly ISerializeService _serializeService = serializeService;
    public override Task<SortPartnersByDistanceReply> SortPartnersByDistance(SortPartnersByDistanceRequest request, ServerCallContext context)
    {
        _logger.Information($"BEGIN: SortPartnersByDistance REQUEST --> {_serializeService.Serialize(request)} <--");
        List<CoordinateDTO> distancesListSorted = DistanceCalculatorHelper.SortCoordinatesByDistance(request.Latitude, request.Longitude, request.Distancedetails.Select(x => new CoordinateDTO
        {
            UserId = x.UserId,
            Latitude = x.Latitude,
            Longitude = x.Longitude
        }).ToList());

        _logger.Information($"END: SortPartnersByDistance RESULT --> {_serializeService.Serialize(distancesListSorted)} <--");

        SortPartnersByDistanceReply result = new()
        {
            Distancedetails = { distancesListSorted.Select(x => new SortPartnersByDistanceReplyDetail
            {
                UserId = x.UserId,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }) }
        };
        return Task.FromResult(result);
    }
}