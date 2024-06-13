namespace Shared.Services.Resources;

public abstract record BaseResourceRequest
{
    public required IFormFile FormFile { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string ContentType => FormFile.ContentType;

    public StoreInfoType Type { get; set; }
}
