namespace Message.Application.Service.Interfaces
{
    public interface ILastMessageChatService
    {
        Task<LastMessageChatEntry> GetLastMessageChatAsync(string currentAccountId, string recipientAccountId);
        Task<MongoPagedList<LastMessageChatDto>> GetListLastMessageChatAsync(string currentUsername, int pageIndex, int pageSize);
        Task<int> GetUnreadAsync(string currentAccountId);
        Task UpdateAsync(LastMessageChatEntry lastMessageChat);
        Task AddAsync(LastMessageChatEntry lastMessageChat);
    }
}
