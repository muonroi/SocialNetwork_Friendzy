namespace User.Application.Feature.v1.Users.Queries.GetUserQuery;

public class GetUserQuery(string Input) : IRequest<ApiResult<UserDto>>
{
    public string Input { get; set; } = Input ?? throw new ArgumentNullException(nameof(Input));

}