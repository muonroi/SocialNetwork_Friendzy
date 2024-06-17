using MongoDB.Driver;
using Shared.Attributes;

namespace Infrastructure.Commons
{
    public class MongoDbRepository<T> : IMongoDbRepositoryBase<T> where T : MongoDbEntity
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly Lazy<IMongoCollection<T>> _collection;
        protected static readonly string? _collectionName;

        static MongoDbRepository()
        {
            _collectionName = typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                                       .FirstOrDefault() is BsonCollectionAttribute attr
                                       ? attr.CollectionName
                                       : null;
        }

        public MongoDbRepository(IMongoClient mongoClient, IConfiguration configuration)
        {
            string databaseName = configuration.GetConfigHelper(ConfigurationSetting.ConnectionMongoDbNameString);
            _mongoDatabase = mongoClient.GetDatabase(databaseName);
            _collection = new Lazy<IMongoCollection<T>>(() =>
                _mongoDatabase.GetCollection<T>(_collectionName ?? throw new InvalidOperationException("Collection name not specified.")));
        }

        public Task CreateAsync(T entity)
        {
            return _collection.Value.InsertOneAsync(entity);
        }

        public Task DeleteAsync(string id)
        {
            return _collection.Value.DeleteOneAsync(x => x.Id == id);
        }

        public IMongoCollection<T> FindAll(ReadPreference? readPreference = null)
        {
            return _mongoDatabase.WithReadPreference(readPreference ?? ReadPreference.Primary)
                                 .GetCollection<T>(_collectionName ?? throw new InvalidOperationException("Collection name not specified."));
        }

        public Task UpdateAsync(T entity)
        {
            PropertyInfo? idProperty = typeof(T).GetProperty(nameof(MongoDbEntity.Id), BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException("Id property not found.");
            string? idValue = (idProperty.GetValue(entity)?.ToString()) ?? throw new InvalidOperationException("Id value is null.");
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(e => e.Id, idValue);
            return _collection.Value.ReplaceOneAsync(filter, entity);
        }
    }
}
