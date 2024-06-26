﻿using Account.Application.Commons.ErrorMessages;
using Account.Application.Commons.Interfaces;
using Account.Application.DTOs;
using Authenticate.Verify.Service;
using Contracts.Commons.Constants;
using Contracts.DTOs.JwtBearerDTOs;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using MediatR;
using Shared.Enums;
using Shared.SeedWorks;
using static Authenticate.Verify.Service.AuthenticateVerify;

namespace Account.Application.Infrastructure.feature.v1.Accounts.Commands.VerifyAccountCommand
{
    public class VerifyAccountCommandHandler(
    GrpcClientFactory grpcClientFactory, IAccountRepository accountRepository, JwtBearerConfig jwtBearerConfig) : IRequestHandler<VerifyAccountCommand, ApiResult<VerifyAccountCommandResponse>>
    {
        private readonly JwtBearerConfig _jwtBearerConfig = jwtBearerConfig ?? throw new ArgumentNullException(nameof(jwtBearerConfig));

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
}
