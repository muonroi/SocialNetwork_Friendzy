using Contracts.Domains;
using Matched.Friend.Domain.Infrastructure.Enums;

namespace Matched.Friend.Domain.Entities;
public class FriendsMatchedEntity : EntityAuditBase<long>
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
    public ActionMatched ActionMatched { get; set; }
}