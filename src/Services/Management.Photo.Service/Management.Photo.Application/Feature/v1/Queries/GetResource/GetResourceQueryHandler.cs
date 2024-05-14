using AutoMapper;
using Infrastructure.Commons;
using Management.Photo.Application.Commons.Interfaces;
using Management.Photo.Application.Commons.Models;
using MediatR;
using Shared.DTOs;
using Shared.SeedWorks;
using System.Net;

namespace Management.Photo.Application.Feature.v1.Queries.GetResource;

public class GetResourceQueryHandler(IMapper mapper, IStoreInfoRepository storeInfoRepository, WorkContextAccessor workContext) : IRequestHandler<GetResourceQuery, ApiResult<GetResourceQueryResponse>>
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private readonly IStoreInfoRepository _storeInfoRepository = storeInfoRepository ?? throw new ArgumentNullException(nameof(storeInfoRepository));

    private readonly WorkContextAccessor _workContext = workContext ?? throw new ArgumentNullException(nameof(workContext));

    public async Task<ApiResult<GetResourceQueryResponse>> Handle(GetResourceQuery request, CancellationToken cancellationToken)
    {
        WorkContextInfoDTO workContext = _workContext.WorkContext!;

        List<StoreInfoDTO>? storeInfo = await _storeInfoRepository.GetResourceByName(workContext.UserId, request.Type);
        return storeInfo is null
            ? new ApiErrorResult<GetResourceQueryResponse>("Store info not found", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<GetResourceQueryResponse>(_mapper.Map<GetResourceQueryResponse>(storeInfo));
    }
}