using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.SeedWorks;

namespace Management.Photo.Application.Feature.v1.Commands.ImportResoure;

public record ImportResourceCommand(StoreInfoType Type, IFormFile File) : IRequest<ApiResult<ImportResourceCommandResponse>>
{
    public StoreInfoType Type { get; set; } = Type;

    public IFormFile File { get; set; } = File;
}