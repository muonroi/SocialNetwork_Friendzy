namespace Post.Service.Services;

public class PostService(ILogger logger, IDapper dapper) : PostApiServiceBase
{
    private readonly ILogger _logger = logger;
    private readonly IDapper _dapper = dapper;

    public override async Task<GetPostApiServiceReply> GetPostApiService(GetPostApiServiceRequest request, ServerCallContext context)
    {
        _logger.Information($"BEGIN: GetPostApiService");
        DapperCommand command = new()
        {
            CommandText = CustomSqlQuery.GetPostByInput,
            Parameters = new
            {
                request.PageIndex,
                request.PageSize
            }
        };
        PageResult<PostApiServiceDetail> postsResult = await _dapper.QueryPageAsync<PostApiServiceDetail>(command, CustomSqlQuery.CountPostByInput, pageIndex: request.PageIndex, pageSite: request.PageSize, cancellationToken: context.CancellationToken);

        if (postsResult.Result.Count < 1)
        {
            return new GetPostApiServiceReply
            {
                CurrentPage = 0,
                HasNextPage = false,
                HasPreviousPage = false,
                PageSize = 0,
                TotalPages = 0,
                TotalRecords = 0
            };
        }

        PagingResponse<IEnumerable<PostApiServiceDetail>> paging = new(postsResult.Result
                                                            , (int)postsResult.TotalCount
                                                            , (int)postsResult.Page
                                                            , request.PageSize);
        GetPostApiServiceReply result = new()
        {
            TotalPages = paging.TotalPages,
            CurrentPage = paging.CurrentPageNumber,
            TotalRecords = paging.TotalRecords,
            PageSize = paging.PageSize,
            HasNextPage = paging.HasNextPage,
            HasPreviousPage = paging.HasPreviousPage,
        };
        result.Details.Add(paging.Data);
        _logger.Information($"END: GetPersonalPostsAsync RESULT --> {JsonConvert.SerializeObject(result)} <-- ");
        return result;
    }
}