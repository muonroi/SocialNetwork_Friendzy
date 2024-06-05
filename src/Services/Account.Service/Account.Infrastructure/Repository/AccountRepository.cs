namespace Account.Infrastructure.Repository;

public class AccountRepository(IMapper mapper, AccountDbContext dbContext, IUnitOfWork<AccountDbContext> unitOfWork, ILogger logger, IDapper dapper) : RepositoryBaseAsync<AccountEntity, Guid, AccountDbContext>(dbContext, unitOfWork), IAccountRepository
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private readonly ILogger _logger = logger;

    private readonly IDapper _dapper = dapper;

    public async Task<Guid> CreateAccountAsync<T>(T account, CancellationToken cancellationToken) where T : AccountDTO
    {
        _logger.Information($"BEGIN: CreateAccountAsync REQUEST --> {JsonConvert.SerializeObject(account)} <-- REQUEST");
        Guid result = Guid.NewGuid();
        _ = await CreateAsync(new AccountEntity
        {
            Id = result,
            AccountType = account.AccountType,
            Currency = account.Currency,
            LockReason = account.LockReason,
            Balance = account.Balance,
            IsActive = account.IsActive,
            IsEmailVerified = account.IsEmailVerified,
            Status = account.Status
        }, cancellationToken);

        _ = await SaveChangesAsync();

        _logger.Information($"END: CreateAccountAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");

        return result;
    }

    public async Task<bool> DeleteAccountAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: DeleteAccountAsync REQUEST --> {id} <-- REQUEST");

        AccountEntity? account = await GetByIdAsync(id);
        if (account is null)
        {
            return false;
        }
        account.IsDeleted = true;
        await UpdateAsync(account);
        long result = await SaveChangesAsync();
        _logger.Information($"END: DeleteAccountAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
        return result > 0;
    }

    public async Task<AccountDTO?> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetAccountByIdAsync REQUEST --> {id} <-- REQUEST");
        DapperCommand command = new()
        {
            CommandText = CustomQuery.GetAccount,
            Parameters = new
            {
                Id = id
            },
        };
        AccountDTO? result = await _dapper.QueryFirstOrDefaultAsync<AccountDTO>(command.Build(cancellationToken));
        if (result is null)
        {
            return null;
        }

        _logger.Information($"END: GetAccountByIdAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");

        return result;
    }

    public async Task<IEnumerable<AccountDTO>?> GetAccountsAsync(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10)
    {
        _logger.Information($"BEGIN: GetAccountsAsync --> {JsonConvert.SerializeObject(new { pageIndex, pageSize })} <-- ");
        DapperCommand command = new()
        {
            CommandText = CustomQuery.GetAccountsPaging,
            Parameters = new
            {
                Offset = (pageIndex - 1) * pageSize,
                PageSize = pageSize
            },
        };
        List<AccountDTO>? rawResult = await _dapper.QueryAsync<AccountDTO>(command.Build(cancellationToken));
        if (rawResult is null)
        {
            return null;
        }
        _logger.Information($"END: GetAccountsAsync RESULT --> {JsonConvert.SerializeObject(rawResult)} <-- ");
        IEnumerable<AccountDTO> result = _mapper.Map<IEnumerable<AccountDTO>>(rawResult);
        return result;
    }

    public async Task<bool> UpdateAccountAsync(Guid id, AccountDTO account, CancellationToken cancellationToken)
    {
        AccountEntity? accountResult = await GetByIdAsync(id);
        if (accountResult is null)
        {
            return false;
        }
        _logger.Information($"BEGIN: UpdateAccountAsync REQUEST --> {JsonConvert.SerializeObject(account)} <-- REQUEST");
        accountResult.RefreshToken = account.RefreshToken;
        accountResult.RefreshTokenExpiryTime = account.RefreshTokenExpiryTime;
        accountResult.AccountType = account.AccountType;
        accountResult.Currency = account.Currency;
        accountResult.LockReason = account.LockReason;
        accountResult.Balance = account.Balance;
        accountResult.IsActive = account.IsActive;
        accountResult.IsEmailVerified = account.IsEmailVerified;
        accountResult.Status = account.Status;

        await UpdateAsync(accountResult);

        long result = await SaveChangesAsync();

        _logger.Information($"END: UpdateAccountAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");

        return result > 0;
    }
}