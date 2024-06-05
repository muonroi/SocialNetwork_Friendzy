namespace Contracts.Commons.Interfaces;

public interface IRepositoryQueryBaseAsync<T, in TK> where T : EntityBase<TK>
{
    IQueryable<T> FindAll(bool trackChanges = false);

    IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties);

    Task<T?> GetByIdAsync(TK id, CancellationToken cancellationToken);

    Task<T?> GetByIdAsync(TK id, params Expression<Func<T, object>>[] includeProperties);

    Task<T?> FindObjectByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
}

public interface IRepositoryQueryBaseAsync<T, in TK, TContext> : IRepositoryQueryBaseAsync<T, TK> where T : EntityBase<TK>
    where TContext : DbContext
{
}