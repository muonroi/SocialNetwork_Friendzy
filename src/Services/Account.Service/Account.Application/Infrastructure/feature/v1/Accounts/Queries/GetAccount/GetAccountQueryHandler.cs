using Account.Application.Commons.ErrorMessages;
using Account.Application.Commons.Interfaces;
using Account.Application.DTOs;
using MediatR;
using Shared.SeedWorks;
using System.Net;

namespace Account.Application.Infrastructure.feature.v1.Accounts.Queries.GetAccount
{
    public class GetAccountQueryHandler(IAccountRepository accountRepository) : IRequestHandler<GetAccountQuery, ApiResult<GetAccountQueryResponse>>
    {
        private readonly IAccountRepository _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

        public async Task<ApiResult<GetAccountQueryResponse>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            AccountDTO? accountResponse = await _accountRepository.GetAccountByIdAsync(request.AccountId, cancellationToken);
            if (accountResponse is null)
            {
                return new ApiErrorResult<GetAccountQueryResponse>($"{AccountErrorMessage.AccountNotFound}", (int)HttpStatusCode.NotFound);
            }
            GetAccountQueryResponse result = new()
            {
                AccountId = accountResponse.Id,
                AccountType = accountResponse.AccountType,
                Currency = accountResponse.Currency,
                LockReason = accountResponse.LockReason,
                Balance = accountResponse.Balance,
                IsActive = accountResponse.IsActive,
                IsEmailVerified = accountResponse.IsEmailVerified,
                Status = accountResponse.Status,
                Roles = accountResponse.Roles
            };

            return result is null
                ? new ApiErrorResult<GetAccountQueryResponse>()
                : new ApiSuccessResult<GetAccountQueryResponse>(result);
        }
    }
}
