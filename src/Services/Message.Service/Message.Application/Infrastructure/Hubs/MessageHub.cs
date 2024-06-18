using Message.Application.Infrastructure.Dtos;
using Message.Application.Service.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Message.Application.Infrastructure.Hubs;

public class MessageHub(IMessageService messageService) : Hub
{
    private readonly IMessageService _messageService = messageService;

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
}