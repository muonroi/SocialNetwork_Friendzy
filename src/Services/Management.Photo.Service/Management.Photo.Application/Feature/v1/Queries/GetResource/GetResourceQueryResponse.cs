using Shared.Enums;

namespace Management.Photo.Application.Feature.v1.Queries.GetResource;

public class GetResourceQueryResponse
{
    public long Id { get; set; }

    public string StoreName { get; set; } = string.Empty;

    public long UserId { get; set; }

    public string StoreDescription { get; set; } = string.Empty;

    public string StoreUrl { get; set; } = string.Empty;

    public StoreInfoType StoreInfoType { get; set; }
}