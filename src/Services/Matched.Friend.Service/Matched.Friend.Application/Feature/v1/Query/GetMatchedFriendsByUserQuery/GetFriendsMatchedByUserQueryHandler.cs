using Commons.Pagination;
using Contracts.Commons.Interfaces;
using ExternalAPI;
using Matched.Friend.Application.Commons.Interfaces;
using Matched.Friend.Application.Commons.Models;
using Matched.Friend.Application.Infrastructure.Helpers;
using Matched.Friend.Application.Messages;
using MediatR;
using Newtonsoft.Json;
using Serilog;
using Shared.DTOs;
using Shared.Resources;
using Shared.SeedWorks;
using System.Net;

namespace Matched.Friend.Application.Feature.v1.Query.GetMatchedFriendsByUserQuery
{
    public class GetFriendsMatchedByUserQueryHandler(IFriendsMatchedRepository friendMatchedRepository,
    IWorkContextAccessor workContextAccessor
    , PaginationConfigs paginationConfigs
    , IApiExternalClient externalClient
    , ILogger logger) : IRequestHandler<GetFriendsMatchedByUserQuery, ApiResult<PagingResponse<IEnumerable<GetFriendsMatchedByUserQueryResponse>>>>
    {
        private readonly IFriendsMatchedRepository _friendMatchedRepository = friendMatchedRepository;

        private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor;

        private readonly ILogger _logger = logger;

        private readonly IApiExternalClient _externalClient = externalClient;

        public async Task<ApiResult<PagingResponse<IEnumerable<GetFriendsMatchedByUserQueryResponse>>>> Handle(GetFriendsMatchedByUserQuery request, CancellationToken cancellationToken)
        {
            WorkContextInfoDTO workContext = _workContextAccessor.WorkContext!;
            if (request.PageIndex < 1)
            {
                request.PageIndex = paginationConfigs.DefaultPageIndex;
            }
            if (request.PageSize < 1)
            {
                request.PageSize = paginationConfigs.DefaultPageSize;
            }
            _logger.Information($"BEGIN: GetFriendsMatchedByUserQuery REQUEST --> {JsonConvert.SerializeObject(request)} <--");

            FriendsMatchedPagingResponse getFriendsMatchedByActionResult = await _friendMatchedRepository.GetFriendsMatchedByAction(workContext.UserId, request.ActionMatched, request.PageIndex, request.PageSize, cancellationToken);

            if (getFriendsMatchedByActionResult.TotalPages == 0)
            {
                return new ApiErrorResult<PagingResponse<IEnumerable<GetFriendsMatchedByUserQueryResponse>>>(nameof(ErrorMessageBase.ToTalPageLessThanOrEqualZero), (int)HttpStatusCode.NotFound);
            }
            if (getFriendsMatchedByActionResult.FriendsMatcheds is null)
            {
                return new ApiErrorResult<PagingResponse<IEnumerable<GetFriendsMatchedByUserQueryResponse>>>(nameof(ErrorMessages.FriendsMatchedsNotFound), (int)HttpStatusCode.NotFound, arguments: request.ActionMatched.ToString());
            }
            PagingResponse<IEnumerable<GetFriendsMatchedByUserQueryResponse>> result = await getFriendsMatchedByActionResult.Mapping(request, _externalClient);

            _logger.Information($"END: GetFriendsMatchedByUserQuery RESULT --> {JsonConvert.SerializeObject(result)} <--");

            return new ApiSuccessResult<PagingResponse<IEnumerable<GetFriendsMatchedByUserQueryResponse>>>(result);

        }
    }
}
