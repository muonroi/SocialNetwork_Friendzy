using Contracts.Commons.Interfaces;
using Dapper.Extensions;
using Grpc.Core;
using Post.Application.Commons.Models;
using Post.Domain.Entities;

namespace Post.Application.Commons.Interfaces;

public interface IPostRepository : IRepositoryBaseAsync<PostEnitity, long>
{
    Task<PageResult<PostDto>> GetRandomPostsAsync(int pageIndex, int pageSize, ServerCallContext context);
}