using Management.Friends.Action.Application.Feature.v1.Command;
using Management.Friends.Action.Application.Feature.v1.Query.GetFriendByUserIdQuery;
using Management.Friends.Action.Application.Feature.v1.Query.GetFriendsActionByUserQuery;
using Management.Friends.Action.Application.Feature.v1.Query.GetFriendsByIdQuery;
using Matched.Friend.Domain.Infrastructure.Enums;

namespace Management.Friends.Action.Service.Controller;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ManagementFriendsActionController(IMediator mediator) : ControllerBase
{
    public readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetFriendsByAction([FromQuery] GetFriendsActionByUserQuery request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpGet("friends")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFriendsById([FromQuery] string id, long userId, int action)
    {
        GetFriendsByIdQuery request = new()
        {
            FriendIds = id.Split(',').Select(long.Parse).ToList(),
            ActionMatched = (ActionMatched)action,
            UserId = userId
        };
        return Ok(await _mediator.Send(request));
    }

    [HttpGet("friends-user")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFriendsById([FromQuery] long userId, int action)
    {
        GetFriendByUserIdQuery request = new()
        {
            ActionMatched = (ActionMatched)action,
            UserId = userId
        };
        return Ok(await _mediator.Send(request));
    }


    #region Command

    [HttpPost]
    public async Task<IActionResult> CreateFriendAction([FromBody] SetFriendByActionCommand request)
    {
        return Ok(await _mediator.Send(request));
    }

    #endregion Command
}