using MongoDB.Driver;

namespace Contracts.Commons.Interfaces
{
    public interface IMongoDbRepositoryBase<T> where T : MongoDbEntity
    {
        IMongoCollection<T> FindAll(ReadPreference? readPreference = null);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
