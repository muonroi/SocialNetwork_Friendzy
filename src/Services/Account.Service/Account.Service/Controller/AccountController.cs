using Account.Application.Infrastructure.feature.v1.Accounts.Commands.CreateAccountCommand;
using Account.Application.Infrastructure.feature.v1.Accounts.Commands.VerifyAccountCommand;
using Account.Application.Infrastructure.feature.v1.Accounts.Queries.GetAccount;
using Account.Application.Infrastructure.feature.v1.Accounts.Queries.GetAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.SeedWorks;
using System.Net;

namespace Account.Service.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet("paging")]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<GetAccountsQueryResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAccounts([FromQuery] GetAccountsQuery request)
        {
            ApiResult<IEnumerable<GetAccountsQueryResponse>> result = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("single")]
        [ProducesResponseType(typeof(ApiResult<GetAccountQueryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAccount([FromQuery] GetAccountQuery request)
        {
            ApiResult<GetAccountQueryResponse> result = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(result);
        }

        #region Command

        [HttpPost]
        [ProducesResponseType(typeof(ApiResult<CreateAccountCommandResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAccounts([FromBody] CreateAccountCommand request)
        {
            ApiResult<CreateAccountCommandResponse> result = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("verify")]
        [ProducesResponseType(typeof(ApiResult<VerifyAccountCommandResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> VerifyAccount([FromBody] VerifyAccountCommand request)
        {
            ApiResult<VerifyAccountCommandResponse> result = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(result);
        }

        #endregion
    }
}
