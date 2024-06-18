using AutoMapper;
using Message.Application.Service.Interfaces;
using Message.Domain.Entities;
using MongoDB.Driver;

namespace Message.Application.Service;

public class GroupService : MongoDbRepository<GroupEntry>, IGroupService
{
    private readonly IMongoCollection<GroupEntry> _groups;
    private readonly IMapper _mapper;
    private readonly IMongoClient _mongoClient;
    private readonly IConfiguration _configuration;

    public GroupService(IMongoClient mongoClient, IConfiguration configuration, IMapper mapper) : base(mongoClient, configuration)
    {
        _mapper = mapper;
        _mongoClient = mongoClient;
        _configuration = configuration;
        IMongoDatabase database = _mongoClient.GetDatabase(_configuration.GetConfigHelper(ConfigurationSetting.ConnectionMongoDbNameString));
        _groups = database.GetCollection<GroupEntry>("Groups");
    }

    // Thêm nhóm
    public async Task AddGroup(GroupEntry group)
    {
        await _groups.InsertOneAsync(group);
    }

    // Lấy nhóm chứa connectionId
    public async Task<GroupEntry> GetGroupForConnection(string connectionId)
    {
        return await _groups.Find(g => g.Connections.Any(c => c.ConnectionId == connectionId)).FirstOrDefaultAsync();
    }

    // Lấy nhóm theo tên
    public async Task<GroupEntry> GetMessageGroup(string groupName)
    {
        return await _groups.Find(g => g.Name == groupName).FirstOrDefaultAsync();
    }
}