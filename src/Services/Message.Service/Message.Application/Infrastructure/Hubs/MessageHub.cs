using Microsoft.AspNetCore.SignalR;

namespace Message.Application.Infrastructure.Hubs
{
    public class MessageHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
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

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ShowWho", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ShowWho", $"{Context.ConnectionId} has left the group {groupName}.");
        }
    }
}
