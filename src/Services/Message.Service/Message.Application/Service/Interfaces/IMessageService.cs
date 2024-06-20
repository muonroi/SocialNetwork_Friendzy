namespace Message.Application.Service.Interfaces;

public interface IMessageService : IMongoDbRepositoryBase<MessageEntry>
{
    Task<MongoPagedList<MessageResponse>> GetMessagePageAsync(string userId, string friendId, int pageIndex, int pageSize);

    Task<bool> PutMessageAsync(Author user, Author friend, MessageDto userMessage, MessageDto friendMessage);

    Task<bool> RemoveMessageAsync(AuthorDto user, AuthorDto friend, MessageDto userMessage, MessageDto friendMessage);

    Task AddMessage(MessageEntry message);

    Task<IEnumerable<MessageResponse>> GetMessageThread(string currentAccountId, string recipientAccountId);

    Task<IEnumerable<MessageResponse>> GetMessageThreadDescending(string currentAccountId, string recipientAccountId);
}