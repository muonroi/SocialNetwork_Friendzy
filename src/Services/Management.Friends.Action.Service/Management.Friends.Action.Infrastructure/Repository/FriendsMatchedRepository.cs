using Management.Friends.Action.Application.Commons.Interfaces;
using Management.Friends.Action.Application.Commons.Models;
using Management.Friends.Action.Domain.Entities;
using Management.Friends.Action.Infrastructure.Persistence;
using Management.Friends.Action.Infrastructure.Persistence.Query;

namespace Management.Friends.Action.Infrastructure.Repository;

public class FriendsMatchedRepository(ManagementFriendsActionDbContext dbContext, IUnitOfWork<ManagementFriendsActionDbContext> unitOfWork, ILogger logger, IDapper dapper, IWorkContextAccessor workContextAccessor, ISerializeService serializeService) : RepositoryBaseAsync<FriendsActionEntity, long, ManagementFriendsActionDbContext>(dbContext, unitOfWork, workContextAccessor), IFriendsActionRepository
{
    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    private readonly ISerializeService _serializeService = serializeService;

    public async Task<IEnumerable<FriendsActionDto>> GetFriendsById(long userId, IEnumerable<long> friendIds, ActionMatched actionMatched, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetFriendsById REQUEST --> {_serializeService.Serialize(new { friendIds = string.Join(',', friendIds), actionMatched })} <--");

        DapperCommand command = new()
        {
            CommandText = CustomSqlQuery.GetFriendsByUserId,
            Parameters = new
            {
                userId,
                friendIds
            }
        };

        List<FriendsActionDto> result = await _dapper.QueryAsync<FriendsActionDto>(command.Build(cancellationToken));

        _logger.Information($"END: GetFriendsById RESULT --> {_serializeService.Serialize(result)} <-- ");

        return result;
    }

    public async Task<FriendsActionPagingResponse> GetFriendsActionByUserId(long userId, ActionMatched actionMatched, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetFriendsActionByUserId REQUEST --> {_serializeService.Serialize(new { pageIndex, pageSize })} <--");
        DapperCommand command = new()
        {
            CommandText = CustomSqlQuery.GetFriendsActionByUserId,
            Parameters = new
            {
                userId,
                actionMatched,
                pageIndex = (pageIndex - 1) * pageSize,
                pageSize,
            }
        };

        PageResult<FriendsByUserIdActionDto> friendsMatchedResult = await _dapper.QueryPageAsync<FriendsByUserIdActionDto>(command, CustomSqlQuery.GetCountFriendsMatchedByUserId, pageIndex: pageIndex, pageSite: pageSize, cancellationToken: cancellationToken);

        if (friendsMatchedResult.Result.Count < 1)
        {
            return new FriendsActionPagingResponse
            {
                CurrentPage = 0,
                HasNextPage = false,
                HasPreviousPage = false,
                PageSize = 0,
                TotalPages = 0,
                TotalRecords = 0
            };
        }

        PagingResponse<IEnumerable<FriendsByUserIdActionDto>> paging = new(friendsMatchedResult.Result
                                                            , (int)friendsMatchedResult.TotalCount
                                                            , (int)friendsMatchedResult.Page
                                                            , pageSize);
        FriendsActionPagingResponse result = new()
        {
            TotalPages = paging.TotalPages,
            CurrentPage = paging.CurrentPageNumber,
            TotalRecords = paging.TotalRecords,
            PageSize = paging.PageSize,
            HasNextPage = paging.HasNextPage,
            HasPreviousPage = paging.HasPreviousPage,
            FriendsActions = []
        };

        result.FriendsActions?.AddRange(paging.Data);

        _logger.Information($"END: GetFriendsActionByUserId RESULT --> {_serializeService.Serialize(friendsMatchedResult)} <-- ");

        return result;
    }

    public async Task<bool> IsExistFriendAction(long userId, long friendId, ActionMatched actionMatched, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: isExistFriendByAction REQUEST --> {_serializeService.Serialize(new { userId, friendId, actionMatched = nameof(actionMatched) })} <--");
        FriendsActionEntity? isExistMatchedByAction = await FindObjectByCondition(x => x.FriendId == friendId && x.UserId == userId);
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
        _ = await CreateAsync(new FriendsActionEntity
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