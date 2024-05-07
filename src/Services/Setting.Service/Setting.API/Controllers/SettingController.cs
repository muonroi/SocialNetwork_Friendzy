namespace Setting.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SettingController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("categories")]
    [ProducesResponseType(typeof(IEnumerable<CategoryData>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCategory()
    {
        GetCategoryQuery request = new(Settings.Category);
        ApiResult<IEnumerable<CategoryData>> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }
}