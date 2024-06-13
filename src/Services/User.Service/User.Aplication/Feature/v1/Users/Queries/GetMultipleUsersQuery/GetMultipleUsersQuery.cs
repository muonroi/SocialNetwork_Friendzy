namespace User.Application.Feature.v1.Users.Queries.GetMultipleUsersQuery;

public class GetMultipleUsersQuery(string Input) : IRequest<ApiResult<IEnumerable<UserDto>>>
{
    public string Input { get; set; } = Input ?? throw new ArgumentNullException(nameof(Input));
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}