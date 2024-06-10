namespace Setting.Application.feature.v1.Queries.GetCategorySettings;

public class GetCategoryQueryHandler(ISettingRepository<SettingEntity, long> settingRepository,
    ISerializeService serializeService) :
    IRequestHandler<GetCategoryQuery, ApiResult<IEnumerable<CategoryDataDTO>>>
{
    private readonly ISettingRepository<SettingEntity, long> _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));

    private readonly ISerializeService _serializeService = serializeService ?? throw new ArgumentNullException(nameof(serializeService));

    public async Task<ApiResult<IEnumerable<CategoryDataDTO>>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        SettingEntity? categoriesSetting = await _settingRepository.GetSettingByType(x => x.Type == request.Type);
        if (categoriesSetting is null)
        {
            return new ApiErrorResult<IEnumerable<CategoryDataDTO>>($"{SettingErrorMessages.CategorySettingNotFound}", (int)HttpStatusCode.NotFound);
        }

        IEnumerable<CategoryDataDTO> result = _serializeService.Deserialize<IEnumerable<CategoryDataDTO>>(categoriesSetting.Content) ?? [];
        return result is null
            ? new ApiErrorResult<IEnumerable<CategoryDataDTO>>($"{SettingErrorMessages.CategorySettingNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<IEnumerable<CategoryDataDTO>>(result);
    }
}