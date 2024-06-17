namespace Management.Friends.Action.Application.Feature.v1.Query.GetFriendsActionByUserQuery;

public class GetFriendsActionByUserQueryHandler(IFriendsActionRepository friendMatchedRepository,
IWorkContextAccessor workContextAccessor
, PaginationConfigs paginationConfigs
, IApiExternalClient externalClient
, ILogger logger
, ISerializeService serializeService) : IRequestHandler<GetFriendsActionByUserQuery, ApiResult<PagingResponse<IEnumerable<GetFriendsActionByUserQueryResponse>>>>
{
    private readonly IFriendsActionRepository _friendMatchedRepository = friendMatchedRepository;

    private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor;

    private readonly ILogger _logger = logger;

    private readonly IApiExternalClient _externalClient = externalClient;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<ApiResult<PagingResponse<IEnumerable<GetFriendsActionByUserQueryResponse>>>> Handle(GetFriendsActionByUserQuery request, CancellationToken cancellationToken)
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
        _logger.Information($"BEGIN: GetFriendsMatchedByUserQuery REQUEST --> {_serializeService.Serialize(request)} <--");

        FriendsActionPagingResponse getFriendsMatchedByActionResult = await _friendMatchedRepository.GetFriendsActionByUserId(workContext.UserId, request.ActionMatched, request.PageIndex, request.PageSize, cancellationToken);

        if (getFriendsMatchedByActionResult.TotalPages == 0)
        {
            return new ApiErrorResult<PagingResponse<IEnumerable<GetFriendsActionByUserQueryResponse>>>(nameof(ErrorMessageBase.ToTalPageLessThanOrEqualZero), (int)HttpStatusCode.NotFound);
        }
        if (getFriendsMatchedByActionResult.FriendsActions is null)
        {
            return new ApiErrorResult<PagingResponse<IEnumerable<GetFriendsActionByUserQueryResponse>>>(nameof(ErrorMessages.FriendsMatchedsNotFound), (int)HttpStatusCode.NotFound, arguments: request.ActionMatched.ToString());
        }
        PagingResponse<IEnumerable<GetFriendsActionByUserQueryResponse>> result = await getFriendsMatchedByActionResult.Mapping(request, _externalClient);

        _logger.Information($"END: GetFriendsMatchedByUserQuery RESULT --> {_serializeService.Serialize(result)} <--");

        return new ApiSuccessResult<PagingResponse<IEnumerable<GetFriendsActionByUserQueryResponse>>>(result);
    }
}