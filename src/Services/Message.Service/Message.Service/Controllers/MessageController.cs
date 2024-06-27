

namespace Message.Service.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MessageController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("last-message/paging")]
    public async Task<IActionResult> GetLastMessages([FromQuery] GetLastMessageQuery request)
    {
        ApiResult<GetLastMessageQueryResponse> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("agora/rtm-token")]
    public async Task<IActionResult> GetRtmToken([FromQuery] GetRtmAgoraTokensQuery request)
    {
        ApiResult<GetRtmAgoraTokensQueryResponse> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("agora/rtc-token")]
    public async Task<IActionResult> GetRtcToken([FromQuery] GetRtcAgoraTokensQuery request)
    {
        ApiResult<GetRtcAgoraTokensQueryResponse> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }
}