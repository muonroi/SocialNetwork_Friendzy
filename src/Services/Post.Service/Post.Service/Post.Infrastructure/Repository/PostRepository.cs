namespace Post.Infrastructure.Repository;

public class PostRepository(PostDbContext dbContext, IUnitOfWork<PostDbContext> unitOfWork, ILogger logger, IDapper dapper)
    : RepositoryBaseAsync<PostEnitity, long, PostDbContext>(dbContext, unitOfWork), IPostRepository
{
    private readonly ILogger _logger = logger;
    private readonly IDapper _dapper = dapper;

    public async Task<PageResult<PostDto>> GetRandomPostsAsync(int pageIndex, int pageSize, ServerCallContext context)
    {
        _logger.Information($"BEGIN: GetPersonalPostsAsync");
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
        _logger.Information($"END: GetPersonalPostsAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
        return result;
    }

    Task<PageResult<PostDto>> IPostRepository.GetRandomPostsAsync(int pageIndex, int pageSize, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}