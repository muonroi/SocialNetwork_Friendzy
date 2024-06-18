namespace Message.Application.Infrastructure.Hubs;

public class MessageHub(IMessageService messageService, IGroupService groupService, IConnectionService connectionService) : Hub
{
    private readonly IMessageService _messageService = messageService;
    private readonly IGroupService _groupService = groupService;
    private readonly IConnectionService _connectionService = connectionService;

    public override async Task OnConnectedAsync()
    {
        HttpContext? httpContext = Context.GetHttpContext();
        string otherUser = httpContext!.Request.Query["user"].ToString();
        string groupName = GetGroupName(Context.UserIdentifier!, otherUser);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        IEnumerable<MessageResponse> messages = await _messageService.GetMessageThread(Context.UserIdentifier!, otherUser);

        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task ReadMessage(string user, string messageId)
    {
        // Logic to mark the message as read
        await Clients.User(user).SendAsync("MessageRead", messageId);
    }

    private string GetGroupName(string caller, string other)
    {
        bool stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }

    //private async Task<GroupEntry> AddToGroup(string groupName)
    //{
    //    var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
    //    ConnectionEntry connection = new(Context.ConnectionId, Context.UserIdentifier!.ToString());
    //    if (group == null)
    //    {
    //        group = new GroupEntry(groupName);
    //        _unitOfWork.MessageRepository.AddGroup(group);
    //    }
    //    group.Connections.Add(connection);

    //    return await _unitOfWork.Complete() ? group : throw new HubException("Failed to join group");
    //}

    //private async Task<GroupEntry> RemoveFromMessageGroup()
    //{
    //    var group = await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
    //    var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
    //    _unitOfWork.MessageRepository.RemoveConnection(connection);

    //    return await _unitOfWork.Complete() ? group : throw new HubException("Fail to remove from group");
    //}
}