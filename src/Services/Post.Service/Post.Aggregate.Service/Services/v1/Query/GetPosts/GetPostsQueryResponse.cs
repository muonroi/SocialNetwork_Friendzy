namespace Post.Aggregate.Service.Services.v1.Query.GetPosts;

public class GetPostsQueryResponse
{
    public long Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }

    public string? AudioUrl { get; set; }

    public string? FileUrl { get; set; }

    public string? Slug { get; set; }

    public bool IsPublished { get; set; }

    public bool IsDeleted { get; set; }

    public long CategoryId { get; set; }
    public long AuthorId { get; set; }

    public UserDataDTO? UserInfo { get; set; }

    public CategoryDataDTO? CategoryInfo { get; set; }
}