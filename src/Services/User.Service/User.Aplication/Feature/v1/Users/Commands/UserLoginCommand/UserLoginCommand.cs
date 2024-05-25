namespace User.Application.Feature.v1.Users.Commands.UserLoginCommand
{
    public record UserLoginCommand(string PhoneNumber) : IRequest<ApiResult<UserDto>>;
}
