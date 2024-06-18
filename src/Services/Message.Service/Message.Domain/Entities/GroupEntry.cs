namespace Message.Domain.Entities;

[BsonCollectionAttribute("Groups")]
public class GroupEntry : MongoDbEntity
{
    public GroupEntry()
    { }

    public GroupEntry(string name)
    {
        Name = name;
    }

    public string Name { get; set; } = string.Empty;

    public ICollection<ConnectionEntry> Connections { get; set; } = [];
}