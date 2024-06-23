namespace Account.Application.Infrastructure.Hubs;

public class StatusAccountHub(PresenceTracker presenceTracker) : Hub
{
    private readonly PresenceTracker _presenceTracker = presenceTracker;

    public override async Task OnConnectedAsync()
    {
        Guid accountId = Guid.Parse(Context.UserIdentifier!);
        long userId = long.Parse(Context.User!.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value);
        _ = await _presenceTracker.UserConnected(accountId, Context.ConnectionId, new CancellationToken());
        Guid[] friendMatchedsOnline = await _presenceTracker.GetOnlineUsersAsync(accountId);
        if (friendMatchedsOnline.Length == 0)
        {
            return;
        }
        IEnumerable<UserDataModel> friendsInfo = await _presenceTracker.GetUsersInfoAsync(friendMatchedsOnline, userId, 0);

        await Clients.Caller.SendAsync("FriendsOnline", friendsInfo);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Guid accountId = Guid.Parse(Context.UserIdentifier!);
        _ = await _presenceTracker.UserDisconnected(accountId, Context.ConnectionId, new CancellationToken());
        await Clients.Others.SendAsync("FriendsOffline", accountId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task CallToAccountId(string otherAccountId, string channelName)
    {
        string currentAccountId = Context.UserIdentifier!;
        Guid[] connections = await _presenceTracker.GetOnlineUsersAsync(Guid.Parse(otherAccountId));
        if (connections != null)
        {
            Console.Write($"HERE {JsonConvert.SerializeObject(connections.Select(x => x.ToString()))}");
            UserDataModel userCalling = await _presenceTracker.GetUserInfo(Guid.Parse(currentAccountId));
            await Clients.User(otherAccountId).SendAsync("DisplayInformationCaller", userCalling, channelName);
            Console.Write($"SEND {JsonConvert.SerializeObject(new { userCalling, channelName })}");
        }
    }
}