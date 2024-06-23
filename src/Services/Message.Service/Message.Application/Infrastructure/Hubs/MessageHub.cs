namespace Message.Application.Infrastructure.Hubs;

public class MessageHub(IMessageService messageService, IGroupService groupService, ILastMessageChatService lastMessageChatService, PresenceTracker presenceTracker, IMapper mapper, IServiceProvider serviceProvider) : Hub
{
    private readonly IGroupService _groupService = groupService;

    private readonly ILastMessageChatService _lastMessageChatService = lastMessageChatService;

    private readonly IMapper _mapper = mapper;

    private readonly IMessageService _messageService = messageService;

    private readonly PresenceTracker _presenceTracker = presenceTracker;

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override async Task OnConnectedAsync()
    {
        HttpContext? httpContext = Context.GetHttpContext();
        string otherUser = httpContext!.Request.Query["friendId"].ToString();
        string groupName = GetGroupName(Context.UserIdentifier!, otherUser);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await AddToGroup(groupName);

        IEnumerable<MessageResponse> messages = await _messageService.GetMessageThread(Context.UserIdentifier!, otherUser);

        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
    }

    public async Task MarkMessageAsRead(string senderAccountId, string recipientAccountId)
    {
        LastMessageChatEntry lastMessage = await _lastMessageChatService.GetLastMessageChatAsync(senderAccountId, recipientAccountId);

        if (lastMessage != null)
        {
            lastMessage.IsRead = true;
            await _lastMessageChatService.UpdateAsync(lastMessage);

            await Clients.User(senderAccountId).SendAsync("MessageRead", recipientAccountId);
        }
    }

    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        IApiExternalClient externalClient = scope.ServiceProvider.GetRequiredService<IApiExternalClient>();

        string? currenAccountId = Context.UserIdentifier!;

        if (string.Equals(currenAccountId, createMessageDto.RecipientAccountId, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new HubException("You cannot send message to yourself");
        }

        UserDataModel? sender = await _presenceTracker.GetAccountInfo(Guid.Parse(currenAccountId), new CancellationToken());

        UserDataModel? recipient = await _presenceTracker.GetAccountInfo(Guid.Parse(createMessageDto.RecipientAccountId), new CancellationToken()) ?? throw new HubException("Not found user");

        MessageEntry message = new()
        {
            Sender = new Author
            {
                FirstName = sender!.FirstName,
                LastName = sender!.LastName,
                ImageUrl = sender!.AvatarUrl,
                Id = sender!.AccountGuid.ToString()
            },
            Recipient = new Author
            {
                FirstName = recipient!.FirstName,
                LastName = recipient!.LastName,
                ImageUrl = recipient!.AvatarUrl,
                Id = recipient!.AccountGuid.ToString()
            },
            SenderAccountId = sender!.AccountGuid.ToString(),
            RecipientAccountId = recipient!.AccountGuid.ToString(),
            Content = createMessageDto.Content
        };

        string groupName = GetGroupName(sender!.AccountGuid.ToString(), recipient!.AccountGuid.ToString());

        GroupEntry group = await _groupService.GetMessageGroup(groupName);

        if (group.Connections.Any(x => x.AccountId == recipient.AccountGuid.ToString()))
        {
            message.DateRead = DateTime.Now;
        }

        await _messageService.AddMessageAsync(message);

        await UpdateLastMessageChat(message);

        await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));

        IEnumerable<string>? connections = await _presenceTracker.GetCurrentConnectionUserOnline(Guid.Parse(createMessageDto.RecipientAccountId), new CancellationToken());

        if (connections != null)
        {
            ExternalApiResponse<UserDataModel> user = await externalClient.GetUserAsync(currenAccountId, CancellationToken.None);

            // gui tin hieu den RecipientUsername, de hien thi chatbox cua userName gui tin nhan
            _ = await externalClient.PushNotificationMessageText(new PushNotificationMessageTextHub { SenderAccountId = currenAccountId, MessageText = createMessageDto.Content, RecipientAccountId = createMessageDto.RecipientAccountId }, CancellationToken.None);
        }

        ////send push notification to user when chat 1-1
        //var toPlayers = await _unitOfWork.OneSignalRepository.GetUserByUsername(createMessageDto.RecipientUsername);

        //List<string> strTemp = [];
        //foreach (var connection in toPlayers.PlayerIds)
        //{
        //    strTemp.Add(connection.PlayerId);
        //}

        //string[] toIds = strTemp.ToArray();

        //if (toIds.Length > 0)
        //{
        //    string messageBody = $"😊 {sender.DisplayName} send a message to you";
        //    var obj = new
        //    {
        //        android_channel_id = _config["OneSignal:AndroidChannelId"],
        //        app_id = _config["OneSignal:AppId"],
        //        headings = new { en = "Social app", es = "Title Spanish Message" },
        //        contents = new { en = messageBody, es = messageBody },
        //        include_subscription_ids = toIds,
        //        name = "INTERNAL_CAMPAIGN_NAME"
        //    };
        //    await _oneSignalService.CreateNotification(obj);
        //}
    }

    /// <summary>
    /// Add last message chat when has message come on
    /// </summary>
    /// <param name="message"></param>
    private async Task UpdateLastMessageChat(MessageEntry message)
    {
        LastMessageChatEntry lastMessageFromDb = await _lastMessageChatService.GetLastMessageChatAsync(message.SenderAccountId, message.RecipientAccountId);

        if (lastMessageFromDb != null)
        {
            lastMessageFromDb.Content = message.Content;
            lastMessageFromDb.MessageLastDate = message.MessageSent;
            await _lastMessageChatService.UpdateAsync(lastMessageFromDb);
        }
        else
        {
            LastMessageChatEntry lastMessageChat = new()
            {
                Content = message.Content,
                MessageLastDate = message.MessageSent,
                Sender = message.Sender,
                Recipient = message.Recipient,
                SenderAccountId = message.SenderAccountId,
                RecipienAccountId = message.RecipientAccountId,
                GroupName = GetGroupName(message.SenderAccountId!, message.RecipientAccountId!)
            };
            await _lastMessageChatService.AddAsync(lastMessageChat);
        }
    }

    private async Task AddToGroup(string groupName)
    {
        GroupEntry? group = await _groupService.GetMessageGroup(groupName);
        ConnectionEntry connection = new(Context.ConnectionId, Context.UserIdentifier!);
        if (group == null)
        {
            group = new GroupEntry(groupName);
            await _groupService.AddGroup(group);
        }
        group.Connections.Add(connection);
    }

    public async Task ReadMessage(string accountId, string messageId)
    {
        // Logic to mark the message as read
        await Clients.User(accountId).SendAsync("MessageRead", messageId);
    }

    private static string GetGroupName(string caller, string other)
    {
        bool stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }
}