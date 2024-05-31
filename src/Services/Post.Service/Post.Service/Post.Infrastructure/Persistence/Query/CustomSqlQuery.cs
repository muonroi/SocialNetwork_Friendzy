namespace Post.Infrastructure.Persistence.Query;

public static class CustomSqlQuery
{
    public const string GetPostByInput = "SELECT  Id, Title, Content, ImageUrl, VideoUrl, AudioUrl, FileUrl, Slug, IsPublished, IsDeleted, CategoryId, AuthorId, CreatedDate FROM Posts WHERE Posts.IsDeleted <> 1 ORDER BY CreatedDateTS LIMIT @pageIndex, @pageSize;";
    public const string CountPostByInput = "SELECT COUNT(*) FROM Posts WHERE Posts.IsDeleted <> 1;";
}