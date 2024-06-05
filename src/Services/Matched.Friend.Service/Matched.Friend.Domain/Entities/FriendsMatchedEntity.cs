namespace Matched.Friend.Domain.Entities;

public class FriendsMatchedEntity : EntityAuditBase<long>
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
    public ActionMatched ActionMatched { get; set; }
}