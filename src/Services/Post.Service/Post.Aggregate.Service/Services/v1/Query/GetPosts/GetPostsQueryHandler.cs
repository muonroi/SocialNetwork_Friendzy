using Commons.Pagination;
using Contracts.Commons.Constants;
using Contracts.Commons.Interfaces;
using ExternalAPI;
using ExternalAPI.DTOs;
using Grpc.Net.ClientFactory;
using MediatR;
using Post.Aggregate.Service.Infrastructure.ErrorMessages;
using Post.Aggregate.Service.Infrastructure.Helpers;
using Post.API.Protos;
using Shared.DTOs;
using Shared.SeedWorks;
using System.Net;
using static Post.API.Protos.PostApiService;

namespace Post.Aggregate.Service.Services.v1.Query.GetPosts;

public class GetPostsQueryHandler(GrpcClientFactory grpcClientFactory
    , IWorkContextAccessor workContextAccessor
    , PaginationConfigs paginationConfigs
    , IApiExternalClient externalClient) : IRequestHandler<GetPostsQuery, ApiResult<PagingResponse<IEnumerable<GetPostsQueryResponse>>>>
{
    private readonly IWorkContextAccessor _workContextAccessor = workContextAccessor;

    private readonly PostApiServiceClient _postApiServiceClient =
        grpcClientFactory.CreateClient<PostApiServiceClient>(ServiceConstants.PostService);

    public async Task<ApiResult<PagingResponse<IEnumerable<GetPostsQueryResponse>>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
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

        CategoryDTO categorySetting = await externalClient.GetCategoryAsync(CancellationToken.None);
        result.Data.ToList().ForEach(x =>
        {
            x.CategoryInfo = categorySetting.Data.FirstOrDefault(c => c.Id == x.CategoryId);
        });

        #endregion Get category info

        #region Get user info

        // if multiple users
        if (result.Data.Count() > 1)
        {
            MultipleUsersDto usersResult = await externalClient.GetUsersAsync(userId, CancellationToken.None);

            result.Data.ToList().ForEach(x =>
            {
                x.UserInfo = usersResult.Data.FirstOrDefault(u => u.Id == x.AuthorId);
            });

            return new ApiSuccessResult<PagingResponse<IEnumerable<GetPostsQueryResponse>>>(result);
        }

        UserDTO userResult = await externalClient.GetUserAsync(userId, CancellationToken.None);
        result.Data.ToList().ForEach(x =>
        {
            x.UserInfo = userResult.Data;
        });

        #endregion Get user info

        return new ApiSuccessResult<PagingResponse<IEnumerable<GetPostsQueryResponse>>>(result);
    }
}