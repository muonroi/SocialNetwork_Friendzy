namespace Message.Application.Service
{
    public class LastMessageChatService : ILastMessageChatService
    {
        private readonly IMongoCollection<LastMessageChatEntry> _lastMessages;
        private readonly IMapper _mapper;
        private readonly IMongoClient _mongoClient;
        private readonly IConfiguration _configuration;

        public LastMessageChatService(IMongoClient mongoClient, IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _mongoClient = mongoClient;
            _configuration = configuration;
            IMongoDatabase database = _mongoClient.GetDatabase(_configuration.GetSection("MongoDbSettings:DatabaseName").Value);
            _lastMessages = database.GetCollection<LastMessageChatEntry>("LastMessageChats");
        }

        public async Task<LastMessageChatEntry> GetLastMessageChatAsync(string currentAccountId, string recipientAccountId)
        {
            FilterDefinition<LastMessageChatEntry> filter = Builders<LastMessageChatEntry>.Filter.Where(x =>
                (x.SenderAccountId == currentAccountId && x.RecipienAccountId == recipientAccountId) ||
                (x.SenderAccountId == recipientAccountId && x.RecipienAccountId == currentAccountId));
            return await _lastMessages.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// group name = hoainam10th-lisa, containGroupName = hoainam10th
        /// </summary>
        /// <param name="currentUsername"></param>
        /// <returns></returns>
        public async Task<List<LastMessageChatDto>> GetListLastMessageChatAsync(string currentUsername)
        {
            FilterDefinition<LastMessageChatEntry> filter = Builders<LastMessageChatEntry>.Filter.Where(x => x.GroupName.Contains(currentUsername));
            List<LastMessageChatEntry> lastMessages = await _lastMessages.Find(filter).ToListAsync();
            return _mapper.Map<List<LastMessageChatEntry>, List<LastMessageChatDto>>(lastMessages);
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
}
