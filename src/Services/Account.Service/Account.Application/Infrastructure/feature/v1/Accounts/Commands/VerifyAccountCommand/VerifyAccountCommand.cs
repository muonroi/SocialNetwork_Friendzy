using Account.Application.DTOs;
using MediatR;
using Shared.SeedWorks;

namespace Account.Application.Infrastructure.feature.v1.Accounts.Commands.VerifyAccountCommand
{
    public class VerifyAccountCommand : AccountDTO, IRequest<ApiResult<VerifyAccountCommandResponse>>
    {
        public long UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
