namespace Matched.Friend.Infrastructure.Repository;

public class FriendsMatchedRepository(FriendsMatchedDbContext dbContext, IUnitOfWork<FriendsMatchedDbContext> unitOfWork, ILogger logger, IDapper dapper, IWorkContextAccessor workContextAccessor, ISerializeService serializeService) : RepositoryBaseAsync<FriendsMatchedEntity, long, FriendsMatchedDbContext>(dbContext, unitOfWork, workContextAccessor), IFriendsMatchedRepository
{
    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<FriendsMatchedPagingResponse> GetFriendsMatchedByAction(long userId, ActionMatched actionMatched, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetFriendsMatchedsByUserId REQUEST --> {_serializeService.Serialize(new { pageIndex, pageSize })} <--");
        DapperCommand command = new()
        {
            CommandText = CustomSqlQuery.GetFriendsMatchedByUserId,
            Parameters = new
            {
                userId,
                actionMatched,
                pageIndex = (pageIndex - 1) * pageSize,
                pageSize,
            }
        };

        PageResult<FriendsMatchedsDto> friendsMatchedResult = await _dapper.QueryPageAsync<FriendsMatchedsDto>(command, CustomSqlQuery.GetCountFriendsMatchedByUserId, pageIndex: pageIndex, pageSite: pageSize, cancellationToken: cancellationToken);

        if (friendsMatchedResult.Result.Count < 1)
        {
            return new FriendsMatchedPagingResponse
            {
                CurrentPage = 0,
                HasNextPage = false,
                HasPreviousPage = false,
                PageSize = 0,
                TotalPages = 0,
                TotalRecords = 0
            };
        }

        PagingResponse<IEnumerable<FriendsMatchedsDto>> paging = new(friendsMatchedResult.Result
                                                            , (int)friendsMatchedResult.TotalCount
                                                            , (int)friendsMatchedResult.Page
                                                            , pageSize);
        FriendsMatchedPagingResponse result = new()
        {
            TotalPages = paging.TotalPages,
            CurrentPage = paging.CurrentPageNumber,
            TotalRecords = paging.TotalRecords,
            PageSize = paging.PageSize,
            HasNextPage = paging.HasNextPage,
            HasPreviousPage = paging.HasPreviousPage,
            FriendsMatcheds = []
        };

        result.FriendsMatcheds?.AddRange(paging.Data);

        _logger.Information($"END: GetFriendsMatchedsByUserId RESULT --> {_serializeService.Serialize(friendsMatchedResult)} <-- ");

        return result;
    }

    public async Task<bool> IsExistFriendAction(long userId, long friendId, ActionMatched actionMatched, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: isExistFriendByAction REQUEST --> {_serializeService.Serialize(new { userId, friendId, actionMatched = nameof(actionMatched) })} <--");
        FriendsMatchedEntity? isExistMatchedByAction = await FindObjectByCondition(x => x.FriendId == friendId && x.UserId == userId);
        if (isExistMatchedByAction is null)
        {
            return true;
        }
        _logger.Information($"END: isExistFriendByAction RESULT --> {_serializeService.Serialize(new { result = true })} <-- ");
        return false;
    }

    public async Task<bool> SetMatchFriendByAction(long userId, long friendId, ActionMatched actionMatched, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: SetMatchFriendByAction REQUEST --> {_serializeService.Serialize(new { userId, friendId, actionMatched = nameof(actionMatched) })} <--");
        _ = await CreateAsync(new FriendsMatchedEntity
        {
            UserId = userId,
            FriendId = friendId,
            ActionMatched = actionMatched
        }, cancellationToken);

        int result = await SaveChangesAsync();
        _logger.Information($"END: SetMatchFriendByAction RESULT --> {_serializeService.Serialize(new { result = result > 0 })} <-- ");
        return result > 0;
    }
}