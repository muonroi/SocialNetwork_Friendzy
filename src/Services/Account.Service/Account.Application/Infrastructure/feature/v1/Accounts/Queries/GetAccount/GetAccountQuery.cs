using MediatR;
using Shared.SeedWorks;

namespace Account.Application.Infrastructure.feature.v1.Accounts.Queries.GetAccount
{
    public class GetAccountQuery : IRequest<ApiResult<GetAccountQueryResponse>>
    {
        public Guid AccountId { get; set; }
    }
}
