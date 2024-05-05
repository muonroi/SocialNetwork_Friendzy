using Contracts.Domains;

namespace Post.Domain.Entities;

public class PostEnitity : EntityAuditBase<long>
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }

    public string? AudioUrl { get; set; }

    public string? FileUrl { get; set; }

    public string? Slug { get; set; }

    public bool IsPublished { get; set; }

    public new bool IsDeleted { get; set; }

    public long? CategoryId { get; set; }

    public long? AuthorId { get; set; }
}