namespace Management.Photo.Service.Controller;

[Route("api/v1/[controller]")]
[ApiController]
//[Authorize]
public class ManagementPhotoController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResult<StoreInfoDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetResourceByType([FromQuery] GetResourceByIdQuery request)
    {
        ApiResult<StoreInfoDTO> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("type")]
    [ProducesResponseType(typeof(ApiResult<IEnumerable<StoreInfoDTO>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetResourceByType([FromQuery] GetResourceQuery request)
    {
        ApiResult<IEnumerable<StoreInfoDTO>> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    #region Command

    [HttpPost("import")]
    [ProducesResponseType(typeof(ApiResult<ImportResourceCommandResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ImportResourceByType([FromForm] ImportResourceCommand request)
    {
        ApiResult<ImportResourceCommandResponse> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("import/multiple")]
    [ProducesResponseType(typeof(ApiResult<IEnumerable<ImportMultipleResourceCommandResponse>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ImportMultipleResource([FromForm] ImportMultipleResourceCommand request)
    {
        ApiResult<IEnumerable<ImportMultipleResourceCommandResponse>> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    #endregion CRUD
}