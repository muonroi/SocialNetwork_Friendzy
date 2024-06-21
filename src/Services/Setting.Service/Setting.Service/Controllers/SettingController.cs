


namespace Setting.Service.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SettingController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("categories")]
    [ProducesResponseType(typeof(IEnumerable<CategoryDataModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCategory([FromQuery] GetCategoryQuery request)
    {
        ApiResult<IEnumerable<CategoryDataModel>> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("user-online")]
    [ProducesResponseType(typeof(IEnumerable<UserOnlineModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserOnline([FromQuery] GetUserOnlineQuery request)
    {
        ApiResult<IEnumerable<UserOnlineModel>> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserOnlineModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateSetting([FromBody] CreateSettingCommand request)
    {
        ApiResult<UserOnlineModel> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }
}