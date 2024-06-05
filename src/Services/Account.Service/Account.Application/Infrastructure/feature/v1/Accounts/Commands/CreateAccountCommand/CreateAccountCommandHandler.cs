namespace Account.Application.Infrastructure.feature.v1.Accounts.Commands.CreateAccountCommand;

public class CreateAccountCommandHandler(
GrpcClientFactory grpcClientFactory, IAccountRepository accountRepository, JwtBearerConfig jwtBearerConfig) : IRequestHandler<CreateAccountCommand, ApiResult<CreateAccountCommandResponse>>
{
    private readonly IAccountRepository _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    private readonly JwtBearerConfig _jwtBearerConfig = jwtBearerConfig ?? throw new ArgumentNullException(nameof(jwtBearerConfig));

    private readonly AuthenticateVerifyClient _authenticateClient =
   grpcClientFactory.CreateClient<AuthenticateVerifyClient>(ServiceConstants.AuthenticateService);

    public async Task<ApiResult<CreateAccountCommandResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        Guid accountIdCreated = await _accountRepository.CreateAccountAsync(request, cancellationToken);

        GenerateTokenReply tokenResult = await GenerateToken(request, accountIdCreated, cancellationToken);
        AccountDTO accountDto = new()
        {
            AccountType = request.AccountType,
            Currency = request.Currency,
            LockReason = request.LockReason,
            Balance = request.Balance,
            IsActive = request.IsActive,
            IsEmailVerified = request.IsEmailVerified,
            Status = request.Status,
            RefreshToken = tokenResult.RefreshToken,
            RefreshTokenExpiryTime = tokenResult.ExpiresIn,
        };
        _ = await _accountRepository.UpdateAccountAsync(accountIdCreated, accountDto, cancellationToken);

        CreateAccountCommandResponse? result = new()
        {
            AccountId = accountIdCreated,
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
        };
        return result is null
            ? new ApiErrorResult<CreateAccountCommandResponse>()
            : new ApiSuccessResult<CreateAccountCommandResponse>(result);
    }

    private async Task<GenerateTokenReply> GenerateToken(CreateAccountCommand request, Guid accountId, CancellationToken cancellationToken)
    {
        GenerateTokenReply tokenResult = await _authenticateClient.GenerateTokenAsync(new GenerateTokenRequest
        {
            GenerateTokenVerify = new GenerateTokenVerify
            {
                SecretKey = _jwtBearerConfig.Key,
                TimeExpires = 365  //change before
            },
            GenerateTokenDetail = new GenerateTokenDetail
            {
                FullName = $"{request.FirstName} {request.LastName}",
                AccountId = accountId.ToString(),
                RoleIds = nameof(RoleConstants.User),
                PhoneNumber = request.PhoneNumber,
                EmailAddress = request.EmailAddress,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                UserId = request.UserId,
                IsActive = request.IsActive,
                Balance = request.Balance.ToString(),
                Currency = (int)request.Currency,
                AccountType = (int)request.AccountType,
                AccountStatus = (int)request.Status,
                IsEmailVerify = request.IsEmailVerified,
            }
        }, cancellationToken: cancellationToken);
        return tokenResult;
    }
}