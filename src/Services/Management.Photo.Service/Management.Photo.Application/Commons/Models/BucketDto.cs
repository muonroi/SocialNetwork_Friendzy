namespace Management.Photo.Application.Commons.Models;

public record BucketDto
{
    public long Id { get; } = 0;
    public string BucketName { get; } = string.Empty;
    public string BucketDescription { get; } = string.Empty;
}