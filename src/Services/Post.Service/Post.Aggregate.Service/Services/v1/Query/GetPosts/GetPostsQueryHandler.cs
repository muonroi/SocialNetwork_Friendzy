﻿namespace Post.Aggregate.Service.Services.v1.Query.GetPosts;

public class GetPostsQueryHandler(GrpcClientFactory grpcClientFactory
    , IWorkContextAccessor workContextAccessor
    , PaginationConfigs paginationConfigs
    , IApiExternalClient externalClient
    , ILogger logger) : IRequestHandler<GetPostsQuery, ApiResult<PagingResponse<IEnumerable<GetPostsQueryResponse>>>>
{
    private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor;
    private readonly ILogger _logger = logger;

    private readonly PostApiServiceClient _postApiServiceClient =
        grpcClientFactory.CreateClient<PostApiServiceClient>(ServiceConstants.PostService);

    public async Task<ApiResult<PagingResponse<IEnumerable<GetPostsQueryResponse>>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: GetPostsQuery REQUEST --> {JsonConvert.SerializeObject(request)} <--");
        WorkContextInfoDTO workContext = _workContextAccessor.WorkContext!;
        if (request.PageIndex < 1)
        {
            request.PageIndex = paginationConfigs.DefaultPageIndex;
        }

        if (request.PageSize < 1 || request.PageSize > paginationConfigs.MaxPageSize)
        {
            request.PageSize = paginationConfigs.DefaultPageSize;
        }

        GetPostApiServiceReply getPostApiServiceResult = await _postApiServiceClient.GetPostApiServiceAsync(new GetPostApiServiceRequest
        {
            UserId = workContext.UserId,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        }, cancellationToken: cancellationToken);

        if (getPostApiServiceResult.TotalPages == 0)
        {
            return new ApiErrorResult<PagingResponse<IEnumerable<GetPostsQueryResponse>>>(nameof(ErrorMessages.PostNotFound), (int)HttpStatusCode.NotFound);
        }

        PagingResponse<IEnumerable<GetPostsQueryResponse>> result = getPostApiServiceResult.Mapping(request);

        // process user id
        string userId = result.Data.Count() > 1 ? string.Join(",", result.Data.Select(x => x.AuthorId)) : result.Data.First().AuthorId.ToString();

        #region Get category info

        ExternalApiResponse<IEnumerable<CategoryDataDTO>> categorySetting = await externalClient.GetCategoryAsync(CancellationToken.None);
        result.Data.ToList().ForEach(x =>
        {
            x.CategoryInfo = categorySetting.Data.FirstOrDefault(c => c.Id == x.CategoryId);
        });

        #endregion Get category info

        #region Get user info

        // if multiple users
        if (result.Data.Count() > 1)
        {
            ExternalApiResponse<IEnumerable<UserDataDTO>> usersResult = await externalClient.GetUsersAsync(userId, CancellationToken.None);

            result.Data.ToList().ForEach(x =>
            {
                x.UserInfo = usersResult.Data.FirstOrDefault(u => u.Id == x.AuthorId);
            });

            return new ApiSuccessResult<PagingResponse<IEnumerable<GetPostsQueryResponse>>>(result);
        }

        ExternalApiResponse<UserDataDTO> userResult = await externalClient.GetUserAsync(userId, CancellationToken.None);
        result.Data.ToList().ForEach(x =>
        {
            x.UserInfo = userResult.Data;
        });

        #endregion Get user info

        _logger.Information($"END: GetPostsQuery RESULT --> {JsonConvert.SerializeObject(result)} <--");

        return new ApiSuccessResult<PagingResponse<IEnumerable<GetPostsQueryResponse>>>(result);
    }
}