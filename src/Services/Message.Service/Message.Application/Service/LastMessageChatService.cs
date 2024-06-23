namespace Message.Application.Service;

public class LastMessageChatService : ILastMessageChatService
{
    private readonly IMongoCollection<LastMessageChatEntry> _lastMessages;
    private readonly IMongoClient _mongoClient;
    private readonly IConfiguration _configuration;
    private readonly IApiExternalClient _externalClient;

    public LastMessageChatService(IMongoClient mongoClient, IConfiguration configuration, IApiExternalClient externalClient)
    {
        _mongoClient = mongoClient;
        _configuration = configuration;
        IMongoDatabase database = _mongoClient.GetDatabase(_configuration.GetConfigHelper(ConfigurationSetting.ConnectionMongoDbNameString));
        _lastMessages = database.GetCollection<LastMessageChatEntry>("LastMessageChats");
        _externalClient = externalClient;
    }

    public async Task<LastMessageChatEntry> GetLastMessageChatAsync(string currentAccountId, string recipientAccountId)
    {
        FilterDefinition<LastMessageChatEntry> filter = Builders<LastMessageChatEntry>.Filter.Where(x =>
            (x.SenderAccountId == currentAccountId && x.RecipienAccountId == recipientAccountId) ||
            (x.SenderAccountId == recipientAccountId && x.RecipienAccountId == currentAccountId));
        return await _lastMessages.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<MongoPagedList<LastMessageChatDto>> GetListLastMessageChatAsync(string currentUserId, int pageIndex, int pageSize)
    {
        FilterDefinition<LastMessageChatEntry> filter = Builders<LastMessageChatEntry>.Filter.Where(x => x.GroupName.Contains(currentUserId));

        SortDefinition<LastMessageChatEntry> sort = Builders<LastMessageChatEntry>.Sort.Descending(x => x.LastModifiedDate);

        MongoPagedList<LastMessageChatEntry> pagedList = await MongoPagedList<LastMessageChatEntry>.ToPagedList(_lastMessages, filter, pageIndex, pageSize, sort);

        List<UserDataModel> userResponse = [];
        string accountIdsRequest = string.Join(",", pagedList.Select(x => x.SenderAccountId));

        accountIdsRequest = string.Concat(accountIdsRequest, ",", string.Join(",", pagedList.Select(x => x.RecipienAccountId)));

        ExternalApiResponse<IEnumerable<UserDataModel>> usersResponse = await _externalClient.GetUsersAsync(accountIdsRequest, CancellationToken.None);

        if (usersResponse?.Data is not null)
        {
            userResponse.AddRange(usersResponse.Data);
        }

        IEnumerable<LastMessageChatDto> items = pagedList.Select(x => new LastMessageChatDto
        {
            Id = x.Id is null ? string.Empty : x.Id.ToString(),
            SenderId = x.SenderId,
            SenderAccountGuid = x.SenderAccountId,
            SenderDisplayName = $"{x.Sender!.FirstName} {x.Sender.LastName}",
            SenderImgUrl = x.Sender.ImageUrl,
            RecipientId = x.RecipientId,
            RecipientAccountGuid = x.RecipienAccountId,
            Content = x.Content,
            MessageLastDate = x.MessageLastDate,
            GroupName = x.GroupName,
            IsRead = x.IsRead,
            Sender = userResponse.FirstOrDefault(y => y.AccountGuid == Guid.Parse(x.SenderAccountId)) ?? new UserDataModel(),
            Recipient = userResponse.FirstOrDefault(y => y.AccountGuid == Guid.Parse(x.RecipienAccountId)) ?? new UserDataModel(),
            QuickSenderInfo = x.Sender,
            QuickRecipientInfo = x.Recipient
        });

        return new MongoPagedList<LastMessageChatDto>(items, pagedList.GetMetaData().TotalItems, pageIndex, pageSize);
    }

    public async Task<int> GetUnreadAsync(string currentUsername)
    {
        FilterDefinition<LastMessageChatEntry> filter = Builders<LastMessageChatEntry>.Filter.Where(x =>
            x.GroupName.Contains(currentUsername) && !x.IsRead);
        List<LastMessageChatEntry> lastMessages = await _lastMessages.Find(filter).ToListAsync();
        return lastMessages.Count;
    }

    public async Task UpdateAsync(LastMessageChatEntry lastMessageChat)
    {
        FilterDefinition<LastMessageChatEntry> filter = Builders<LastMessageChatEntry>.Filter.Eq(x => x.Id, lastMessageChat.Id);
        _ = await _lastMessages.ReplaceOneAsync(filter, lastMessageChat);
    }

    public async Task AddAsync(LastMessageChatEntry lastMessageChat)
    {
        await _lastMessages.InsertOneAsync(lastMessageChat);
    }
}