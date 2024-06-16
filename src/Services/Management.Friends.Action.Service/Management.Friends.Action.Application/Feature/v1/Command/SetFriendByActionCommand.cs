namespace Management.Friends.Action.Application.Feature.v1.Command;

public class SetFriendByActionCommand : IRequest<ApiResult<bool>>
{
    public long FriendId { get; set; }
    public ActionMatched ActionMatched { get; set; }
}