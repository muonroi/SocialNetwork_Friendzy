namespace Management.Photo.Application.Feature.v1.Commands.ImportMultipleResource;

public record ImportMultipleResourceCommand(StoreInfoType Type, IEnumerable<IFormFile> Files) : IRequest<ApiResult<IEnumerable<ImportMultipleResourceCommandResponse>>>
{
    public StoreInfoType Type { get; set; } = Type;
    public IEnumerable<IFormFile> Files { get; set; } = Files;
}