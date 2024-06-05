namespace Account.Application.Infrastructure.feature.v1.Accounts.Queries.GetAccounts;

public class GetAccountsQueryHandler(IAccountRepository accountRepository) : IRequestHandler<GetAccountsQuery, ApiResult<IEnumerable<GetAccountsQueryResponse>>>
{
    private readonly IAccountRepository _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

    public async Task<ApiResult<IEnumerable<GetAccountsQueryResponse>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<AccountDTO>? accountResponse = await _accountRepository.GetAccountsAsync(cancellationToken, request.PageIndex, request.PageSize);
        if (accountResponse is null)
        {
            return new ApiErrorResult<IEnumerable<GetAccountsQueryResponse>>($"{AccountErrorMessage.AccountNotFound}", (int)HttpStatusCode.NotFound);
        }
        IEnumerable<GetAccountsQueryResponse>? result = accountResponse.Select(account => new GetAccountsQueryResponse
        {
            AccountId = account.Id,
            AccountType = account.AccountType,
            Currency = account.Currency,
            LockReason = account.LockReason,
            Balance = account.Balance,
            IsActive = account.IsActive,
            IsEmailVerified = account.IsEmailVerified,
            Status = account.Status,
            Roles = account.Roles
        });

        return result is null
            ? new ApiErrorResult<IEnumerable<GetAccountsQueryResponse>>()
            : new ApiSuccessResult<IEnumerable<GetAccountsQueryResponse>>(result);
    }
}