﻿namespace Setting.Service.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SettingController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("categories")]
    [ProducesResponseType(typeof(IEnumerable<CategoryDataDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCategory()
    {
        GetCategoryQuery request = new(Settings.Category);
        ApiResult<IEnumerable<CategoryDataDTO>> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }
}