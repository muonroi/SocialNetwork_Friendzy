﻿using Distance.Service.Protos;
using ExternalAPI.Models;
using SearchPartners.Service;
using static Distance.Service.Protos.DistanceService;
using static SearchPartners.Service.SearchPartnerService;

namespace SearchPartners.Aggregate.Service.Services.v1.Query.SearchPartners;

public class SearchPartnersQueryHandler(
    GrpcClientFactory grpcClientFactory
    , IWorkContextAccessor workContextAccessor
    , IApiExternalClient externalClient
    , ISerializeService serializeService) : IRequestHandler<SearchPartnersQuery, ApiResult<SearchPartnersQueryResponse>>
{
    private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor;

    private readonly IApiExternalClient _externalClient = externalClient;

    private readonly ISerializeService _serializeService = serializeService;

    private readonly SearchPartnerServiceClient _searchPartnersClient =
        grpcClientFactory.CreateClient<SearchPartnerServiceClient>(ServiceConstants.SearchPartnersService);

    private readonly DistanceServiceClient _distanceServiceClient =
        grpcClientFactory.CreateClient<DistanceServiceClient>(ServiceConstants.DistanceService);

    public async Task<ApiResult<SearchPartnersQueryResponse>> Handle(SearchPartnersQuery request, CancellationToken cancellationToken)
    {
        WorkContextInfoDTO workContext = _workContextAccessor.WorkContext!;
        SearchPartnersQueryResponse result = new();

        GetDistanceInfoListReply distanceResult = await _distanceServiceClient.GetDistanceInfoListAsync(new GetDistanceInfoListRequest
        {
            Country = request.Country,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        }, cancellationToken: cancellationToken);

        SortPartnersByDistanceReply partnersResult = _searchPartnersClient.SortPartnersByDistance(new SortPartnersByDistanceRequest
        {
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Distancedetails = {
            distanceResult.DistanceInfoList.Select(x => new DistanceDetail
            {
                UserId = x.UserId,
                Latitude = x.Latitude,
                Longitude = x.Longitude
                })
            }
        }, cancellationToken: cancellationToken);

        if (partnersResult.Distancedetails.Count == 0)
        {
            return new ApiErrorResult<SearchPartnersQueryResponse>(nameof(SearchPartnersErrorMessages.PartnersNotFound), StatusCodes.Status404NotFound);
        }
        string userId = partnersResult.Distancedetails.Count > 1 ? string.Join(",", partnersResult.Distancedetails.Select(x => x.UserId)) : partnersResult.Distancedetails.First().UserId.ToString();

        if (partnersResult.Distancedetails.Count > 1)
        {
            ExternalApiResponse<IEnumerable<UserDataModel>> usersResult = await _externalClient.GetUsersAsync(userId, CancellationToken.None);
            if (!usersResult.IsSucceeded)
            {
                return new ApiErrorResult<SearchPartnersQueryResponse>(nameof(SearchPartnersErrorMessages.PartnersNotFound), StatusCodes.Status404NotFound);
            }
            result = new()
            {
                Id = workContext.UserId,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                PartnersSorted = new PagedList<UserDataModel>(
                   usersResult.Data,
                   distanceResult.TotalItems,
                   request.PageIndex,
                   request.PageSize),
            };
            return new ApiSuccessResult<SearchPartnersQueryResponse>(result);
        }

        ExternalApiResponse<UserDataModel> userResult = await _externalClient.GetUserAsync(partnersResult.Distancedetails.First().UserId.ToString(), CancellationToken.None);
        result = new()
        {
            Id = workContext.UserId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            PartnersSorted = new PagedList<UserDataModel>(
                [
                    userResult.Data
                ],
               distanceResult.TotalItems,
               request.PageIndex,
               request.PageSize)
        };
        return new ApiSuccessResult<SearchPartnersQueryResponse>(result);
    }
}