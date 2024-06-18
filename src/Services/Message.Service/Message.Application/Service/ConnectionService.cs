namespace Message.Application.Service;

public class ConnectionService : MongoDbRepository<ConnectionEntry>, IConnectionService
{
    private readonly IMongoCollection<ConnectionEntry> _connections;
    private readonly IMapper _mapper;
    private readonly IMongoClient _mongoClient;
    private readonly IConfiguration _configuration;

    public ConnectionService(IMongoClient mongoClient, IConfiguration configuration, IMapper mapper) : base(mongoClient, configuration)
    {
        _mapper = mapper;
        _mongoClient = mongoClient;
        _configuration = configuration;
        IMongoDatabase database = _mongoClient.GetDatabase(_configuration.GetConfigHelper(ConfigurationSetting.ConnectionMongoDbNameString));
        _connections = database.GetCollection<ConnectionEntry>("Connections");
    }

    public async Task RemoveConnection(ConnectionEntry connection)
    {
        await _connections.DeleteOneAsync(c => c.ConnectionId == connection.ConnectionId);
    }

    // Lấy kết nối theo connectionId
    public async Task<ConnectionEntry> GetConnection(string connectionId)
    {
        return await _connections.Find(c => c.ConnectionId == connectionId).FirstOrDefaultAsync();
    }
}