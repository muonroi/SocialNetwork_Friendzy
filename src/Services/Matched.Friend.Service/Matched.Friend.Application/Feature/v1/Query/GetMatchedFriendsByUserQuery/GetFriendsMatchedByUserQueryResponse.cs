namespace Matched.Friend.Application.Feature.v1.Query.GetMatchedFriendsByUserQuery;

public class GetFriendsMatchedByUserQueryResponse
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
    public UserDataDTO? FriendData { get; set; }
    public ActionMatched ActionMatched { get; set; }
}