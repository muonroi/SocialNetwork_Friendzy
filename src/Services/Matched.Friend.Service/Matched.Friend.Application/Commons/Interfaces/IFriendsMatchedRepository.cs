using Contracts.Commons.Interfaces;
using Matched.Friend.Application.Commons.Models;
using Matched.Friend.Domain.Entities;
using Matched.Friend.Domain.Infrastructure.Enums;

namespace Matched.Friend.Application.Commons.Interfaces;

public interface IFriendsMatchedRepository : IRepositoryBaseAsync<FriendsMatchedEntity, long>
{
    Task<FriendsMatchedPagingResponse> GetFriendsMatchedByAction(long userId, ActionMatched actionMatched, int pageIndex, int pageSize, CancellationToken cancellationToken);

    Task<bool> isExistFriendAction(long userId, long friendId, ActionMatched actionMatched, CancellationToken cancellationToken);

    Task<bool> SetMatchFriendByAction(long userId, long friendId, ActionMatched actionMatched, CancellationToken cancellationToken);
}