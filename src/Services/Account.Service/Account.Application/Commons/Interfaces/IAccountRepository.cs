namespace Account.Application.Commons.Interfaces;

public interface IAccountRepository
{
    Task<AccountDTO?> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<AccountDTO>?> GetAccountsAsync(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10);

    Task<Guid> CreateAccountAsync<T>(T account, CancellationToken cancellationToken) where T : AccountDTO;

    Task<bool> UpdateAccountAsync(Guid id, AccountDTO account, CancellationToken cancellationToken);

    Task<bool> DeleteAccountAsync(Guid id, CancellationToken cancellationToken);
}