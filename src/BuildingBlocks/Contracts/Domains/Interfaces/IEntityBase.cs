namespace Contracts.Domains.Interfaces;

public interface IEntityBase<TKey>
{
    TKey Id { get; set; }
}