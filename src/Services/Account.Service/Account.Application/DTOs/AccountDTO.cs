using Account.Application.Mappings;
using Account.Domain.Entities;
using Shared.Enums;

namespace Account.Application.DTOs
{
    public class AccountDTO : IMapFrom<AccountEntity>
    {
        public Guid Id { get; set; }

        public AccountType AccountType { get; set; }

        public Currency Currency { get; set; }

        public string LockReason { get; set; } = string.Empty;

        public virtual decimal Balance { get; set; }

        public bool IsActive { get; set; }

        public bool IsEmailVerified { get; set; }

        public AccountStatus Status { get; set; }

        public string Roles { get; set; } = string.Empty;
    }
}
