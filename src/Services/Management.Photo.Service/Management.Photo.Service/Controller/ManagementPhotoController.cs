using Management.Photo.Application.Feature.v1.Queries.GetResource;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.SeedWorks;
using System.Net;

namespace Management.Photo.Service.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class ManagementPhotoController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("type")]
    [ProducesResponseType(typeof(ApiResult<GetResourceQueryResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetResourceByType([FromQuery] GetResourceQuery request)
    {
        ApiResult<GetResourceQueryResponse> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }
}