namespace Management.Friends.Action.Application.Commons.Models;

public record FriendsByUserIdActionDto
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
    public UserDataModel? UserData { get; set; }
    public UserDataModel? FriendData { get; set; }
    public ActionMatched ActionMatched { get; set; }
}