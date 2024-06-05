namespace Management.Photo.Application.Commons.Models;

public class StoreInfoDTO
{
    public string StoreName { get; set; } = string.Empty;

    public long UserId { get; set; }

    public string StoreDescription { get; set; } = string.Empty;

    public string StoreUrl { get; set; } = string.Empty;

    public string BucketName { get; } = string.Empty;

    public string BucketDescription { get; } = string.Empty;

    public StoreInfoType StoreInfoType { get; set; }
}