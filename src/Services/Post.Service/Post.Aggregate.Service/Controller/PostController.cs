namespace Post.Aggregate.Service.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class PostController(IMediator mediator) : ControllerBase
{
    public readonly IMediator _mediator = mediator;

    [HttpGet("get-posts")]
    public async Task<IActionResult> GetPosts([FromQuery] GetPostsQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
}