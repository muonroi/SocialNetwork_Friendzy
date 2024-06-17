using AutoMapper;
using Commons.Pagination;
using Message.Infrastructure.Dtos;
using Message.Infrastructure.Service.Interfaces;

namespace Message.Infrastructure.Service
{
    public class MessageService : MongoDbRepository<MessageEntry>, IMessageService
    {
        private readonly IMongoCollection<MessageEntry> _messageCollection;
        private readonly IMapper _mapper;
        private readonly IMongoClient _mongoClient;
        private readonly IConfiguration _configuration;

        public MessageService(IMongoClient mongoClient, IConfiguration configuration, IMapper mapper) : base(mongoClient, configuration)
        {
            _mapper = mapper;
            _mongoClient = mongoClient;
            _configuration = configuration;
            IMongoDatabase database = mongoClient.GetDatabase(_configuration.GetConfigHelper(ConfigurationSetting.ConnectionMongoDbNameString));
            _messageCollection = database.GetCollection<MessageEntry>(nameof(MessageEntry));
        }

        public async Task<MongoPagedList<MessageResponse>> GetMessagePageAsync(string userId, string friendId, int pageIndex, int pageSize)
        {
            FilterDefinition<MessageEntry> filter = Builders<MessageEntry>.Filter.Where(x =>
                (x.RecipientId == userId && x.SenderId == friendId) ||
                (x.RecipientId == friendId && x.SenderId == userId));

            SortDefinition<MessageEntry> sort = Builders<MessageEntry>.Sort.Descending(x => x.MessageSent);

            MongoPagedList<MessageEntry> pagedList = await MongoPagedList<MessageEntry>.ToPagedList(_messageCollection, filter, pageIndex, pageSize, sort);

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
    }
}
