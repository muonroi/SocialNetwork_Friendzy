namespace Post.Aggregate.Service.Services.v1.Query.GetPosts;

public record GetPostsQuery : IRequest<ApiResult<PagingResponse<IEnumerable<GetPostsQueryResponse>>>>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}