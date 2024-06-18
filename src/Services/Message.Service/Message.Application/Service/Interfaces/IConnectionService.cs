namespace Message.Application.Service.Interfaces;

public interface IConnectionService : IMongoDbRepositoryBase<ConnectionEntry>
{
    Task<ConnectionEntry> GetConnection(string connectionId);

    Task RemoveConnection(ConnectionEntry connection);
}