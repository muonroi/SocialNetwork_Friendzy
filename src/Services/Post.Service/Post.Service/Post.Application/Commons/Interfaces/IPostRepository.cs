namespace Post.Application.Commons.Interfaces;

public interface IPostRepository : IRepositoryBaseAsync<PostEnitity, long>
{
    Task<PageResult<PostDto>> GetRandomPostsAsync(int pageIndex, int pageSize, long userId, ServerCallContext context);
}