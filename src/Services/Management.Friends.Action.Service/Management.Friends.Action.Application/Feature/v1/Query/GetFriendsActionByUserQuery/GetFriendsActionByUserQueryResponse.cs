namespace Management.Friends.Action.Application.Feature.v1.Query.GetFriendsActionByUserQuery;

public class GetFriendsActionByUserQueryResponse
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
    public UserDataModel? FriendData { get; set; }
    public ActionMatched ActionMatched { get; set; }
}