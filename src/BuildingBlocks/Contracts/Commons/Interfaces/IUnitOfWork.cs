namespace Contracts.Commons.Interfaces;

public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
{
    Task<int> CommitAsync();
}