namespace Account.Application.Feature.v1.Accounts.Queries.GetAccountsPaging;

public class GetAccountsPagingQueryHandler(IAccountRepository accountRepository) : IRequestHandler<GetAccountsPagingQuery, ApiResult<IEnumerable<GetAccountsPagingQueryResponse>>>
{
    private readonly IAccountRepository _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

    public async Task<ApiResult<IEnumerable<GetAccountsPagingQueryResponse>>> Handle(GetAccountsPagingQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<AccountDTO>? accountResponse = await _accountRepository.GetAccountsAsync(cancellationToken, request.PageIndex, request.PageSize);
        if (accountResponse is null)
        {
            return new ApiErrorResult<IEnumerable<GetAccountsPagingQueryResponse>>($"{AccountErrorMessage.AccountNotFound}", (int)HttpStatusCode.NotFound);
        }
        IEnumerable<GetAccountsPagingQueryResponse>? result = accountResponse.Select(account => new GetAccountsPagingQueryResponse
        {
            AccountId = account.Id,
            AccountType = account.AccountType,
            Currency = account.Currency,
            LockReason = account.LockReason,
            Balance = account.Balance,
            IsActive = account.IsActive,
            IsEmailVerified = account.IsEmailVerified,
            Status = account.Status,
            Roles = account.Roles,
            LastModifiedDate = account.LastModifiedDate.DateTime.GetTimeStamp(true)
        });

        return result is null
            ? new ApiErrorResult<IEnumerable<GetAccountsPagingQueryResponse>>()
            : new ApiSuccessResult<IEnumerable<GetAccountsPagingQueryResponse>>(result);
    }
}