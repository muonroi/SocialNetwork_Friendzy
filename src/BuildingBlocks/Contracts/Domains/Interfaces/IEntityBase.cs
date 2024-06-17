using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Contracts.Domains.Interfaces;

public interface IEntityBase<T>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? MongoId { get; set; }

    T Id { get; set; }
}