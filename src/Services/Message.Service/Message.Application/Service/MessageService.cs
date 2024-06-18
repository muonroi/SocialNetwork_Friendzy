namespace Message.Application.Service;

public class MessageService : MongoDbRepository<MessageEntry>, IMessageService
{
    private readonly IMongoCollection<MessageEntry> _messages;
    private readonly IMapper _mapper;
    private readonly IMongoClient _mongoClient;
    private readonly IConfiguration _configuration;

    public MessageService(IMongoClient mongoClient, IConfiguration configuration, IMapper mapper) : base(mongoClient, configuration)
    {
        _mapper = mapper;
        _mongoClient = mongoClient;
        _configuration = configuration;
        IMongoDatabase database = _mongoClient.GetDatabase(_configuration.GetConfigHelper(ConfigurationSetting.ConnectionMongoDbNameString));
        _messages = database.GetCollection<MessageEntry>("Messages");
    }

    public async Task<MongoPagedList<MessageResponse>> GetMessagePageAsync(string userId, string friendId, int pageIndex, int pageSize)
    {
        FilterDefinition<MessageEntry> filter = Builders<MessageEntry>.Filter.Where(x =>
            (x.RecipientId == userId && x.SenderId == friendId) ||
            (x.RecipientId == friendId && x.SenderId == userId));

        SortDefinition<MessageEntry> sort = Builders<MessageEntry>.Sort.Descending(x => x.MessageSent);

        MongoPagedList<MessageEntry> pagedList = await MongoPagedList<MessageEntry>.ToPagedList(_messages, filter, pageIndex, pageSize, sort);

        IEnumerable<MessageResponse> items = _mapper.Map<IEnumerable<MessageResponse>>(pagedList);

        return new MongoPagedList<MessageResponse>(items, pagedList.GetMetaData().TotalItems, pageIndex, pageSize);
    }

    public async Task<bool> PutMessageAsync(Author user, Author friend, MessageDto userMessage, MessageDto friendMessage)
    {
        MessageEntry chatAdd = new()
        {
            SenderAccountId = user.Id,
            RecipientAccountId = friend.Id,
            Content = userMessage.Content,
            Sender = user,
            Recipient = friend,
            MessageSent = userMessage.MessageSent
        };
        await CreateAsync(chatAdd);

        return true;
    }

    public Task<bool> RemoveMessageAsync(AuthorDto user, AuthorDto friend, MessageDto userMessage, MessageDto friendMessage)
    {
        throw new NotImplementedException();
    }

    // Thêm tin nhắn
    public async Task AddMessage(MessageEntry message)
    {
        await _messages.InsertOneAsync(message);
    }

    // Lấy chuỗi tin nhắn
    public async Task<IEnumerable<MessageResponse>> GetMessageThread(string currentAccountId, string recipientAccountId)
    {
        List<MessageEntry> messages = await _messages.Find(m =>
            (m.Recipient!.Id == currentAccountId && m.Sender!.Id == recipientAccountId) ||
            (m.Recipient.Id == recipientAccountId && m.Sender!.Id == currentAccountId))
            .SortBy(m => m.MessageSent)
            .ToListAsync();

        IEnumerable<MessageResponse> messageResponses = _mapper.Map<IEnumerable<MessageResponse>>(messages);

        List<MessageResponse> unreadMessages = messageResponses.Where(m => m.DateRead == null && m.RecipientAccountId == currentAccountId).ToList();

        if (unreadMessages.Count != 0)
        {
            foreach (MessageResponse? mess in unreadMessages)
            {
                mess.DateRead = DateTime.Now;
            }
        }

        return messageResponses;
    }

    // Lấy chuỗi tin nhắn theo thứ tự giảm dần
    public async Task<IEnumerable<MessageResponse>> GetMessageThreadDescending(string currentAccountId, string recipientAccountId)
    {
        // Tạo bộ lọc cho truy vấn
        FilterDefinition<MessageEntry> filter = Builders<MessageEntry>.Filter.Where(m =>
            (m.Recipient!.Id == currentAccountId && m.Sender!.Id == recipientAccountId) ||
            (m.Recipient.Id == recipientAccountId && m.Sender!.Id == currentAccountId)
        );

        // Lấy danh sách tin nhắn và sắp xếp theo thứ tự giảm dần
        List<MessageEntry> messages = await _messages.Find(filter)
                                      .SortByDescending(m => m.MessageSent)
                                      .ToListAsync();

        IEnumerable<MessageResponse> messageResponses = _mapper.Map<IEnumerable<MessageResponse>>(messages);

        List<MessageResponse> unreadMessages = messageResponses.Where(m => m.DateRead == null && m.RecipientAccountId == currentAccountId).ToList();

        if (unreadMessages.Count != 0)
        {
            foreach (MessageResponse? mess in unreadMessages)
            {
                mess.DateRead = DateTime.Now;
            }
        }

        return messageResponses;
    }
}