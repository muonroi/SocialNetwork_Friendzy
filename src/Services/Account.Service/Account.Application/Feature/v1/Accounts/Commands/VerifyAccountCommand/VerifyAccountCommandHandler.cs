namespace Account.Application.Feature.v1.Accounts.Commands.VerifyAccountCommand;

public class VerifyAccountCommandHandler(
        GrpcClientFactory grpcClientFactory,
        IAccountRepository accountRepository,
        JwtBearerConfig jwtBearerConfig,
        IHubContext<StatusAccountHub> statusAccount,
        PresenceTracker presenceTracker)
    : IRequestHandler<VerifyAccountCommand,
        ApiResult<VerifyAccountCommandResponse>>
{
    private readonly PresenceTracker _presenceTracker = presenceTracker;
    private readonly JwtBearerConfig _jwtBearerConfig = jwtBearerConfig ?? throw new ArgumentNullException(nameof(jwtBearerConfig));
    private readonly IHubContext<StatusAccountHub> _statusAccount = statusAccount ?? throw new ArgumentNullException(nameof(statusAccount));

    private readonly AuthenticateVerifyClient _authenticateClient =
   grpcClientFactory.CreateClient<AuthenticateVerifyClient>(ServiceConstants.AuthenticateService);

    public async Task<ApiResult<VerifyAccountCommandResponse>> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
    {
        AccountDTO? accountInfo = await accountRepository.GetAccountByIdAsync(request.Id, cancellationToken);
        if (accountInfo == null)
        {
            return new ApiErrorResult<VerifyAccountCommandResponse>(nameof(AccountErrorMessage.AccountNotFound), (int)StatusCode.NotFound);
        }

        GenerateTokenReply tokenResult = await GenerateToken(request, accountInfo.Id, cancellationToken);

        VerifyAccountCommandResponse? result = new()
        {
            AccountId = accountInfo.Id,
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
        };
        if (result is not null)
        {
            Guid[] friendMatchedsOnline = await _presenceTracker.GetOnlineUsersAsync(accountInfo.Id);
            if (friendMatchedsOnline.Length > 0)
            {
                IEnumerable<UserDataModel> friendsInfo = await _presenceTracker.GetUsersInfoAsync(friendMatchedsOnline, request.UserId, 0);
                List<UserDataModel> currentUserModels = [];
                UserDataModel currentUserModel = new()
                {
                    AccountGuid = accountInfo.Id,
                    Id = request.UserId,
                    PhoneNumber = request.PhoneNumber,
                    EmailAddress = request.EmailAddress,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    ProfileImages = request.ProfileImages.ToArray(),
                    AvatarUrl = request.AvatarUrl,
                    Gender = (int)request.Gender,
                    BirthDate = request.Birthdate.ToDateTime(),
                    LastModifiedDateTs = request.LastModifiedDateTs.ToDateTime(),
                };
                currentUserModels.Add(currentUserModel);

                await _statusAccount.Clients.Users(new ReadOnlyCollection<string>(friendsInfo.Select(x => x.AccountGuid.ToString()).ToList())).SendAsync("FriendsOnlinePersonal", currentUserModels, cancellationToken);
            }
        }
        return result is null
            ? new ApiErrorResult<VerifyAccountCommandResponse>()
            : new ApiSuccessResult<VerifyAccountCommandResponse>(result);
    }

    private async Task<GenerateTokenReply> GenerateToken(VerifyAccountCommand request, Guid accountId, CancellationToken cancellationToken)
    {
        GenerateTokenReply tokenResult = await _authenticateClient.GenerateTokenAsync(new GenerateTokenRequest
        {
            GenerateTokenVerify = new GenerateTokenVerify
            {
                SecretKey = _jwtBearerConfig.Key,
                Audience = _jwtBearerConfig.Audience,
                Issuer = _jwtBearerConfig.Issuer,
                TimeExpires = 365,  //change before
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