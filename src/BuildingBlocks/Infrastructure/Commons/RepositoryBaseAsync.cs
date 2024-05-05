namespace Infrastructure.Commons;

public class RepositoryBaseAsync<T, TK, TContext>(TContext context, IUnitOfWork<TContext> unitOfWork) : RepositoryQueryBaseAsync<T, TK, TContext>(context),
    IRepositoryBaseAsync<T, TK, TContext>
    where T : EntityBase<TK> where TContext : DbContext
{
    private readonly TContext _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IUnitOfWork<TContext> _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<TK> CreateAsync(T entity)
    {
        _ = await _context.Set<T>().AddAsync(entity);
        return entity.Id;
    }

    public async Task<IList<TK>> CreateListAsync(IEnumerable<T> entities)
    {
        T[] entityBases = entities as T[] ?? entities.ToArray();
        await _context.Set<T>().AddRangeAsync(entityBases);
        return entityBases.Select(x => x.Id).ToList();
    }

    public Task UpdateAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Unchanged)
        {
            return Task.CompletedTask;
        }

        T? exist = _context.Set<T>().Find(entity.Id);
        if (exist is null)
        {
            return Task.CompletedTask;
        }

        _context.Entry(exist).CurrentValues.SetValues(entity);
        return Task.CompletedTask;
    }

    public Task UpdateListAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _ = _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteListAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync()
    {
        return _unitOfWork.CommitAsync();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return _context.Database.BeginTransactionAsync();
    }

    public async Task EndTransactionAsync()
    {
        _ = await SaveChangesAsync();
        await _context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        return _context.Database.RollbackTransactionAsync();
    }

    Task<IDbContextTransaction> IRepositoryBaseAsync<T, TK>.BeginTransactionAsync()
    {
        throw new NotImplementedException();
    }
}