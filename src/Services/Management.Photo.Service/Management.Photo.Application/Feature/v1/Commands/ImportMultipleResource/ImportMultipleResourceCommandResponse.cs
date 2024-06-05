namespace Management.Photo.Application.Feature.v1.Commands.ImportMultipleResource;

public class ImportMultipleResourceCommandResponse
{
    public string StoreName { get; set; } = string.Empty;

    public long UserId { get; set; }

    public string StoreDescription { get; set; } = string.Empty;

    public string StoreUrl { get; set; } = string.Empty;

    public StoreInfoType StoreInfoType { get; set; }
}