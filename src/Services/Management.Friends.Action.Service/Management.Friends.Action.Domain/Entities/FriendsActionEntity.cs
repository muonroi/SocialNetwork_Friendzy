namespace Management.Friends.Action.Domain.Entities;

public class FriendsActionEntity : EntityAuditBase<long>
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
    public ActionMatched ActionMatched { get; set; }
}