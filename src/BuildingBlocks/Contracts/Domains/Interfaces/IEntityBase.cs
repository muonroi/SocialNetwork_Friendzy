namespace Contracts.Domains.Interfaces;

public interface IEntityBase<TKey>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    TKey Id { get; set; }
}