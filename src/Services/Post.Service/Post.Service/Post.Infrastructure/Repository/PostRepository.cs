namespace Post.Infrastructure.Repository;

public class PostRepository(PostDbContext dbContext, IUnitOfWork<PostDbContext> unitOfWork, ILogger logger, IDapper dapper)
    : RepositoryBaseAsync<PostEnitity, long, PostDbContext>(dbContext, unitOfWork), IPostRepository
{
    private readonly ILogger _logger = logger;
    private readonly IDapper _dapper = dapper;

    public async Task<PageResult<PostDto>> GetRandomPostsAsync(int pageIndex, int pageSize, ServerCallContext context)
    {
        _logger.Information($"BEGIN: GetRandomPostsAsync REQUEST --> {JsonConvert.SerializeObject(new { pageIndex, pageSize })} <--");
        DapperCommand command = new()
        {
            CommandText = CustomSqlQuery.GetPostByInput,
            Parameters = new
            {
                pageIndex,
                pageSize,
            }
        };
        PageResult<PostDto> result = await _dapper.QueryPageAsync<PostDto>(command, CustomSqlQuery.CountPostByInput, pageIndex: pageIndex, pageSite: pageSize, cancellationToken: context.CancellationToken);
        _logger.Information($"END: GetRandomPostsAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
        return result;
    }
}