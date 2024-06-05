namespace Matched.Friend.Application.Commons.Interfaces;

public interface IFriendsMatchedRepository : IRepositoryBaseAsync<FriendsMatchedEntity, long>
{
    Task<FriendsMatchedPagingResponse> GetFriendsMatchedByAction(long userId, ActionMatched actionMatched, int pageIndex, int pageSize, CancellationToken cancellationToken);

    Task<bool> IsExistFriendAction(long userId, long friendId, ActionMatched actionMatched, CancellationToken cancellationToken);

    Task<bool> SetMatchFriendByAction(long userId, long friendId, ActionMatched actionMatched, CancellationToken cancellationToken);
}