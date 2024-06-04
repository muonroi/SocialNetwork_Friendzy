using Authenticate.Verify.Service;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Authenticate.Verify.Service.AuthenticateVerify;
using ILogger = Serilog.ILogger;
namespace Authenticate.Service.Services
{
    public class AuthenticateService(ILogger logger) : AuthenticateVerifyBase
    {
        private readonly ILogger _logger = logger;

        public override async Task<GenerateTokenReply> GenerateToken(GenerateTokenRequest request, ServerCallContext context)
        {
            _logger.Information($"BEGIN: GenerateToken called. REQUEST --> {JsonConvert.SerializeObject(request)} <--");
            DateTimeOffset refreshTokenExpriyTime = DateTime.UtcNow.AddDays(request.GenerateTokenVerify.TimeExpires);
            GenerateTokenDetail tokenInfo = request.GenerateTokenDetail;
            string secretKey = request.GenerateTokenVerify.SecretKey;
            if (secretKey.Length < 32)
            {
                // Pad the key to ensure it's at least 32 bytes
                secretKey = secretKey.PadRight(32, '0');
            }
            byte[] jwtKey = Convert.FromBase64String(secretKey);

            // Create the claims for the access token
            List<Claim> claims =
            [
                new(ClaimTypes.MobilePhone, tokenInfo.PhoneNumber ?? string.Empty),
                new(ClaimTypes.Email, tokenInfo.EmailAddress ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Iss, request.GenerateTokenVerify.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, request.GenerateTokenVerify.Audience),
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
                new("FullName", tokenInfo.FullName.ToString()),

            ];

            SecurityTokenDescriptor accessTokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = refreshTokenExpriyTime.UtcDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityTokenDescriptor refreshTokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", tokenInfo.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature)
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
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(request.SecretKey))
            };

            VerifyTokenReply verifyTokenReply = new();

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
            return verifyTokenReply;
        }
    }
}
