namespace Management.Photo.Application.Feature.v1.Commands.ImportResoure;

public record ImportResourceCommand(StoreInfoType Type, IFormFile File, int Index) : IRequest<ApiResult<ImportResourceCommandResponse>>
{
    public StoreInfoType Type { get; set; } = Type;

    public IFormFile File { get; set; } = File;
}