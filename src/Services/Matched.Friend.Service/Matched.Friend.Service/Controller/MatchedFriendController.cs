namespace Matched.Friend.Service.Controller;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class MatchedFriendController(IMediator mediator) : ControllerBase
{
    public readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetFriendsByAction([FromQuery] GetFriendsMatchedByUserQuery request)
    {
        return Ok(await _mediator.Send(request));
    }

    #region Command

    [HttpPost]
    public async Task<IActionResult> CreateFriendAction([FromBody] SetMatchFriendCommand request)
    {
        return Ok(await _mediator.Send(request));
    }

    #endregion Command
}