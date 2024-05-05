namespace User.Application.Feature.v1.Users.Queries.GetMultipleUsers;

public class GetMultipleUsersQuery(string Input) : IRequest<ApiResult<IEnumerable<UserDto>>>
{
    public string Input { get; set; } = Input ?? throw new ArgumentNullException(nameof(Input));
}