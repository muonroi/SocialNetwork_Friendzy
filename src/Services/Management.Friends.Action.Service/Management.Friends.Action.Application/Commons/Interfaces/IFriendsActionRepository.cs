using Management.Friends.Action.Application.Commons.Models;
using Management.Friends.Action.Domain.Entities;

namespace Management.Friends.Action.Application.Commons.Interfaces;

public interface IFriendsActionRepository : IRepositoryBaseAsync<FriendsActionEntity, long>
{
    Task<FriendsActionPagingResponse> GetFriendsActionByUserId(long userId, ActionMatched actionMatched, int pageIndex, int pageSize, CancellationToken cancellationToken);

    Task<IEnumerable<FriendsActionDto>> GetFriendsById(long userId, IEnumerable<long> friendIds, ActionMatched actionMatched, CancellationToken cancellationToken);

    Task<bool> IsExistFriendAction(long userId, long friendId, ActionMatched actionMatched, CancellationToken cancellationToken);

    Task<bool> SetMatchFriendByAction(long userId, long friendId, ActionMatched actionMatched, CancellationToken cancellationToken);
}