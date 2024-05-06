namespace SearchPartners.Aggregate.Service.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class SearchPartnerController(IMediator mediator) : ControllerBase
{
    public readonly IMediator _mediator = mediator;

    [HttpGet("searchPartners")]
    public async Task<IActionResult> SearchPartners([FromQuery] SearchPartnersQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
}