namespace Message.Application.Service.Interfaces
{
    public interface ILastMessageChatService
    {
        Task<LastMessageChatEntry> GetLastMessageChatAsync(string currentAccountId, string recipientAccountId);
        Task<List<LastMessageChatDto>> GetListLastMessageChatAsync(string currentAccountId);
        Task<int> GetUnreadAsync(string currentAccountId);
        Task UpdateAsync(LastMessageChatEntry lastMessageChat);
        Task AddAsync(LastMessageChatEntry lastMessageChat);
    }
}
