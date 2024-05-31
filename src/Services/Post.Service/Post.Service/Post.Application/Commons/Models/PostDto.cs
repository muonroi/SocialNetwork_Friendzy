namespace Post.Application.Commons.Models;

public class PostDto
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

    public CategoryDataDTO? Category { get; set; }

    public UserDataDTO? Author { get; set; }
}