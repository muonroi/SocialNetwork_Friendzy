using User.Application.Feature.v1.Users.Commands.UserUpdateInfoCommand;
using User.Application.Feature.v1.Users.Queries.GetUserQuery;

namespace User.Service.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResult<UserDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserByKey([FromQuery] string input)
    {
        GetUserQuery request = new(input);
        ApiResult<UserDto> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("list")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResult<IEnumerable<UserDto>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserByKeys([FromQuery] string input)
    {
        GetMultipleUsersQuery request = new(input);
        ApiResult<IEnumerable<UserDto>> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    #region Command

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResult<UserDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Login([FromQuery] string input)
    {
        UserLoginCommand request = new(input);
        ApiResult<UserDto> result = await _mediator.Send(request).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResult<UserDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Register([FromBody] UserRegisterCommand cmd)
    {
        ApiResult<UserDto> result = await _mediator.Send(cmd).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPut("info")]
    [ProducesResponseType(typeof(ApiResult<UserDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update([FromBody] UserUpdateInfoCommand cmd)
    {
        ApiResult<UserDto> result = await _mediator.Send(cmd).ConfigureAwait(false);
        return Ok(result);
    }

    #endregion Command
}