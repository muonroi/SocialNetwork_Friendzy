namespace Contracts.Domains;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    [BsonId]
    public TKey Id { get; set; } = default!;

    public bool IsDeleted { get; set; }
}