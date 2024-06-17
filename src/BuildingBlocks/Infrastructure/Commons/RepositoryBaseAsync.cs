namespace Infrastructure.Commons;

public class RepositoryBaseAsync<T, TK, TContext>(TContext context, IUnitOfWork<TContext> unitOfWork, IWorkContextAccessor workContextAccessor) : RepositoryQueryBaseAsync<T, TK, TContext>(context),
    IRepositoryBaseAsync<T, TK, TContext>
    where T : EntityAuditBase<TK> where TContext : DbContext
{
    private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor;

    private readonly TContext _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IUnitOfWork<TContext> _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<TK> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        entity.CreatedBy = _workContextAccessor?.WorkContext!.UserId.ToString();

        _ = await _context.Set<T>().AddAsync(entity, cancellationToken);

        return entity.Id;
    }

    public async Task<IList<TK>> CreateListAsync(IEnumerable<T> entities)
    {
        T[] entityBases = entities as T[] ?? entities.ToArray();

        foreach (T entity in entityBases)
        {
            entity.CreatedBy = _workContextAccessor?.WorkContext!.UserId.ToString();
        }
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

        exist.LastModifiedBy = _workContextAccessor?.WorkContext!.UserId.ToString();

        _context.Entry(exist).CurrentValues.SetValues(entity);
        return Task.CompletedTask;
    }

    public Task UpdateListAsync(IEnumerable<T> entities)
    {
        foreach (T entity in entities)
        {
            entity.LastModifiedBy = _workContextAccessor?.WorkContext!.UserId.ToString();
        }

        _context.Set<T>().UpdateRange(entities);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        entity.DeletedBy = _workContextAccessor?.WorkContext!.UserId.ToString();

        _ = _context.Set<T>().Remove(entity);

        return Task.CompletedTask;
    }

    public Task DeleteListAsync(IEnumerable<T> entities)
    {
        foreach (T entity in entities)
        {
            entity.DeletedBy = _workContextAccessor?.WorkContext!.UserId.ToString();
        }
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
}