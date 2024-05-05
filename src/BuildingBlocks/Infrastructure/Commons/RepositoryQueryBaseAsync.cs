namespace Infrastructure.Commons;

public class RepositoryQueryBaseAsync<T, TK, TContext>(TContext dbContext) : IRepositoryQueryBaseAsync<T, TK, TContext>
    where T : EntityBase<TK>
    where TContext : DbContext
{
    private readonly TContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        return !trackChanges ? _dbContext.Set<T>().Where(x => !x.IsDeleted).AsNoTracking() :
            _dbContext.Set<T>();
    }

    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> items = FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        return !trackChanges
            ? _dbContext.Set<T>().Where(expression).Where(x => !x.IsDeleted).AsNoTracking()
            : _dbContext.Set<T>().Where(expression).Where(x => !x.IsDeleted);
    }

    public async Task<T?> FindObjectByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        return await FindByCondition(expression)
             .FirstOrDefaultAsync();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public async Task<T?> GetByIdAsync(TK id)
    {
        return await FindByCondition(x => x.Id != null && x.Id.Equals(id) && !x.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<T?> GetByIdAsync(TK id, params Expression<Func<T, object>>[] includeProperties)
    {
        return await FindByCondition(x => x.Id != null && x.Id.Equals(id), trackChanges: false, includeProperties)
            .FirstOrDefaultAsync();
    }
}