using Grpc.Core;
using Newtonsoft.Json;
using SearchPartners.Service.Helpers;
using Shared.DTOs;
using static SearchPartners.Service.SearchPartnerService;

namespace SearchPartners.Service.Services;

public class SearchPartnersService(ILogger<SearchPartnersService> logger) : SearchPartnerServiceBase
{
    private readonly ILogger<SearchPartnersService> _logger = logger;

    public override Task<SortPartnersByDistanceReply> SortPartnersByDistance(SortPartnersByDistanceRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"BEGIN: SortPartnersByDistance");
        List<CoordinateDTO> distancesListSorted = DistanceCalculatorHelper.SortCoordinatesByDistance(request.Latitude, request.Longitude, request.Distancedetails.Select(x => new CoordinateDTO
        {
            UserId = x.UserId,
            Latitude = x.Latitude,
            Longitude = x.Longitude
        }).ToList());
        _logger.LogInformation(message: $"END: SortPartnersByDistance -- RESULT -- {JsonConvert.SerializeObject(distancesListSorted)}");

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