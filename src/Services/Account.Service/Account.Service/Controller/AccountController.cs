



namespace Account.Service.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountController(IMediator mediator, IHubContext<StatusAccountHub> statusAccountHub) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    private readonly IHubContext<StatusAccountHub> _statusAccountHub = statusAccountHub ?? throw new ArgumentNullException(nameof(statusAccountHub));

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

    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResult<CreateAccountCommandResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand request)
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

    [HttpPost("send-notification-text")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> SendNotificationMessage([FromBody] PushNotificationMessageTextHub request)
    {
        await _statusAccountHub.Clients.All.SendAsync("NewMessageReceived", request.AccountId, request.MessageText);
        return Ok();
    }

    #endregion Command
}