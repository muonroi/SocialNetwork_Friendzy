namespace Message.Application.Service.Interfaces;

public interface IGroupService : IMongoDbRepositoryBase<GroupEntry>
{
    Task AddGroup(GroupEntry group);

    Task<GroupEntry> GetGroupForConnection(string connectionId);

    Task<GroupEntry> GetMessageGroup(string groupName);
}