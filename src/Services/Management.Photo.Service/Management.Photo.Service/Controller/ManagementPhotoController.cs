using Management.Photo.Application.Commons.Models;
using Management.Photo.Application.Feature.v1.Commands.ImportMultipleResource;
using Management.Photo.Application.Feature.v1.Commands.ImportResoure;
using Management.Photo.Application.Feature.v1.Queries.GetResource;
using Management.Photo.Application.Feature.v1.Queries.GetResourceById;
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

    #region CRUD

    [HttpPost("import")]
    [ProducesResponseType(typeof(ApiResult<ImportResourceCommandResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ImportResourceByType([FromForm] ImportResourceCommand request)
    {
        ApiResult<ImportResourceCommandResponse> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("import/multiple")]
    [ProducesResponseType(typeof(ApiResult<IEnumerable<ImportMultipleResourceCommandResponse>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ImportMultipleResource([FromQuery] ImportMultipleResourceCommand request)
    {
        ApiResult<IEnumerable<ImportMultipleResourceCommandResponse>> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    #endregion
}