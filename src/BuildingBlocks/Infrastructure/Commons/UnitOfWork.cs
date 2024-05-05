namespace Infrastructure.Commons;

public class UnitOfWork<TContext>(TContext context) : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context = context;

    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync(new CancellationToken());
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}