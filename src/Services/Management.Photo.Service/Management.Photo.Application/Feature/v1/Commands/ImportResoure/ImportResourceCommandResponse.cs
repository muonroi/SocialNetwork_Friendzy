using Shared.Enums;

namespace Management.Photo.Application.Feature.v1.Commands.ImportResoure;

public class ImportResourceCommandResponse
{
    public string StoreName { get; set; } = string.Empty;

    public long UserId { get; set; }

    public string StoreDescription { get; set; } = string.Empty;

    public string StoreUrl { get; set; } = string.Empty;

    public StoreInfoType StoreInfoType { get; set; }
}