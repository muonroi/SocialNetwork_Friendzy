namespace Contracts.Commons.Interfaces;

public interface IRepositoryBaseAsync<T, TK> : IRepositoryQueryBaseAsync<T, TK>
    where T : EntityBase<TK>
{
    Task<TK> CreateAsync(T entity, CancellationToken cancellationToken);

    Task<IList<TK>> CreateListAsync(IEnumerable<T> entities);

    Task UpdateAsync(T entity);

    Task UpdateListAsync(IEnumerable<T> entities);

    Task DeleteAsync(T entity);

    Task DeleteListAsync(IEnumerable<T> entities);

    Task<int> SaveChangesAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task EndTransactionAsync();

    Task RollbackTransactionAsync();
}

public interface IRepositoryBaseAsync<T, TK, TContext> : IRepositoryBaseAsync<T, TK>
    where T : EntityBase<TK>
    where TContext : DbContext
{
}