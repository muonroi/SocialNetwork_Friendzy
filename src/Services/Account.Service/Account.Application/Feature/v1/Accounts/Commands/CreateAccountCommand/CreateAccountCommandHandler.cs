namespace Account.Application.Feature.v1.Accounts.Commands.CreateAccountCommand;

public class CreateAccountCommandHandler(
GrpcClientFactory grpcClientFactory, IAccountRepository accountRepository, IAccountRoleRepository accountRoleRepository, JwtBearerConfig jwtBearerConfig) : IRequestHandler<CreateAccountCommand, ApiResult<CreateAccountCommandResponse>>
{
    private readonly IAccountRepository _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

    private readonly IAccountRoleRepository _accountRoleRepository = accountRoleRepository ?? throw new ArgumentNullException(nameof(accountRoleRepository));

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

        //assign account to role user
        //E897718A-9D7E-4AAA-1E81-08DC8DAB5820
        //F9E297B9-15D2-4DD6-61FC-08DC781F4659
        _ = await _accountRoleRepository.AssignAccountToRoleId(accountIdCreated, Guid.Parse("E897718A-9D7E-4AAA-1E81-08DC8DAB5820"), cancellationToken); // edit after, this is role user hardcode

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
                TimeExpires = 365,  //change before
                Audience = _jwtBearerConfig.Audience,
                Issuer = _jwtBearerConfig.Issuer,
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