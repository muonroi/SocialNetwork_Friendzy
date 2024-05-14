using AutoMapper;
using Management.Photo.Application.Commons.Interfaces;
using Management.Photo.Domain.Entities;
using MediatR;
using Shared.SeedWorks;
using System.Net;

namespace Management.Photo.Application.Feature.v1.Commands.ImportResoure;

public class ImportResourceCommandHandler(IMapper mapper, IStoreInfoRepository storeInfoRepository) : IRequestHandler<ImportResourceCommand, ApiResult<ImportResourceCommandResponse>>
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IStoreInfoRepository _storeInfoRepository = storeInfoRepository ?? throw new ArgumentNullException(nameof(storeInfoRepository));

    public async Task<ApiResult<ImportResourceCommandResponse>> Handle(ImportResourceCommand request, CancellationToken cancellationToken)
    {
        StoreInfoEntity? storeInfo = await _storeInfoRepository.GetByIdAsync(1);
        return storeInfo is null
            ? new ApiErrorResult<ImportResourceCommandResponse>("Store info not found", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<ImportResourceCommandResponse>(_mapper.Map<ImportResourceCommandResponse>(storeInfo));
    }
}