﻿namespace Authenticate.Service.Services;

public class AuthenticateService(ILogger logger, ISerializeService serializeService) : AuthenticateVerifyBase
{
    private readonly ILogger _logger = logger;

    private readonly ISerializeService _serializeService = serializeService;

    public override async Task<GenerateTokenReply> GenerateToken(GenerateTokenRequest request, ServerCallContext context)
    {
        _logger.Information($"BEGIN: GenerateToken called. REQUEST --> {_serializeService.Serialize(request)} <--");
        DateTimeOffset refreshTokenExpriyTime = DateTime.UtcNow.AddDays(request.GenerateTokenVerify.TimeExpires);
        GenerateTokenDetail tokenInfo = request.GenerateTokenDetail;
        string secretKey = request.GenerateTokenVerify.SecretKey;
        if (secretKey.Length < 32)
        {
            secretKey = secretKey.PadRight(32, '0');
        }
        byte[] jwtKey = Encoding.ASCII.GetBytes(secretKey);

        List<Claim> claims =
        [
            new(ClaimTypes.MobilePhone, tokenInfo.PhoneNumber ?? string.Empty),
            new(ClaimTypes.Email, tokenInfo.EmailAddress ?? string.Empty),
            new(ClaimTypes.NameIdentifier, tokenInfo.AccountId.ToString()),
            new(JwtRegisteredClaimNames.Iss, request.GenerateTokenVerify.Issuer),
            new(JwtRegisteredClaimNames.Aud, request.GenerateTokenVerify.Audience),
            new("Roles",string.Join(",", tokenInfo.RoleIds)),
            new("Latitude", tokenInfo.Latitude.ToString()),
            new("Longitude", tokenInfo.Longitude.ToString()),
            new("UserId", tokenInfo.UserId.ToString()),
            new("IsActive", tokenInfo.IsActive.ToString()),
            new("Balance", tokenInfo.Balance.ToString()),
            new("IsEmailVerify", tokenInfo.IsEmailVerify.ToString()),
            new("AccountStatus", tokenInfo.AccountStatus.ToString()),
            new("Currency", tokenInfo.Currency.ToString()),
            new("AccountType", tokenInfo.AccountType.ToString()),
            new("FullName", tokenInfo.FullName.ToString())
        ];

        // Thêm kid vào header
        SigningCredentials signingCredentials = new(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature);
        signingCredentials.Key.KeyId = secretKey;

        SecurityTokenDescriptor accessTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = refreshTokenExpriyTime.UtcDateTime,
            SigningCredentials = signingCredentials
        };

        SecurityTokenDescriptor refreshTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
            new Claim("UserId", tokenInfo.UserId.ToString())
            ]),
            Expires = DateTime.UtcNow.AddYears(1),
            SigningCredentials = signingCredentials
        };

        JwtSecurityTokenHandler tokenHandler = new();

        SecurityToken accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
        SecurityToken refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

        string accessTokenResult = tokenHandler.WriteToken(accessToken);
        string refreshTokenResult = tokenHandler.WriteToken(refreshToken);

        _logger.Information($"END: GenerateToken called. AccessToken --> {accessTokenResult} <--, RefreshToken --> {refreshTokenResult} <--");

        return await Task.FromResult(new GenerateTokenReply
        {
            AccessToken = accessTokenResult,
            RefreshToken = refreshTokenResult,
            ExpiresIn = refreshTokenExpriyTime.ToUnixTimeSeconds()
        });
    }

    public override async Task<VerifyTokenReply> VerifyToken(VerifyTokenRequest request, ServerCallContext context)
    {
        string token = request.AccessToken.Replace("Bearer ", string.Empty);

        JwtSecurityTokenHandler tokenHandler = new();
        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = request.Issuer,
            ValidAudience = request.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(request.SecretKey))
        };

        VerifyTokenReply verifyTokenReply = new();

        try
        {
            TokenValidationResult claimsPrincipal = await tokenHandler.ValidateTokenAsync(token, validationParameters);

            verifyTokenReply.IsAuthenticated = claimsPrincipal.IsValid;

            if (!claimsPrincipal.IsValid)
            {
                return verifyTokenReply;
            }
            JwtPayload payload = new JwtSecurityTokenHandler().ReadJwtToken(token).Payload;
            verifyTokenReply.FullName = payload["FullName"]?.ToString() ?? string.Empty;
            verifyTokenReply.PhoneNumber = payload[ClaimTypes.MobilePhone]?.ToString() ?? string.Empty;
            verifyTokenReply.EmailAddress = payload["email"]?.ToString() ?? string.Empty;
            verifyTokenReply.RoleIds = payload["Roles"]?.ToString() ?? string.Empty;
            verifyTokenReply.Latitude = double.Parse(payload["Latitude"]?.ToString()!);
            verifyTokenReply.Longitude = double.Parse(payload["Longitude"]?.ToString()!);
            verifyTokenReply.UserId = int.Parse(payload["UserId"]?.ToString()!);
            verifyTokenReply.IsActive = bool.Parse(payload["IsActive"]?.ToString()!);
            verifyTokenReply.Balance = payload["Balance"]?.ToString()!;
            verifyTokenReply.IsEmailVerify = bool.Parse(payload["IsEmailVerify"]?.ToString()!);
            verifyTokenReply.AccountStatus = int.Parse(payload["AccountStatus"]?.ToString()!);
            verifyTokenReply.Currency = int.Parse(payload["Currency"]?.ToString()!);
            verifyTokenReply.AccountType = (int)double.Parse(payload["AccountType"]?.ToString()!);
            verifyTokenReply.AccountId = payload["nameid"]?.ToString()!;
        }
        catch (Exception ex)
        {
            _logger.Error($"Token validation failed: {ex.Message}");
            verifyTokenReply.IsAuthenticated = false;
        }

        return verifyTokenReply;
    }
}