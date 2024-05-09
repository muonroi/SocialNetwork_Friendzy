using MediatR;
using Shared.Enums;
using Shared.SeedWorks;

namespace Management.Photo.Application.Feature.v1.Commands
{
    public class ImportResourceCommand : IRequest<ApiResult<ImportResourceCommandResponse>>
    {
        public string StoreName { get; set; } = string.Empty;

        public string StoreDescription { get; set; } = string.Empty;

        public string StoreUrl { get; set; } = string.Empty;

        public StoreInfoType Type { get; set; }
    }
}