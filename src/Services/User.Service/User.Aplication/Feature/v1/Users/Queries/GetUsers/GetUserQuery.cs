namespace User.Application.Feature.v1.Users.Queries.GetUsers;

public class GetUsersQuery(string Input) : IRequest<ApiResult<UserDto>>
{
    public string Input { get; set; } = Input ?? throw new ArgumentNullException(nameof(Input));
}