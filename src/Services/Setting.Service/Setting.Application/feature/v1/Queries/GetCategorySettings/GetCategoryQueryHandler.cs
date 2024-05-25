namespace Setting.Application.feature.v1.Queries.GetCategorySettings;

public class GetCategoryQueryHandler(ISettingRepository<SettingEntity, long> settingRepository) :
    IRequestHandler<GetCategoryQuery, ApiResult<IEnumerable<CategoryDataDTO>>>
{
    private readonly ISettingRepository<SettingEntity, long> _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));

    public async Task<ApiResult<IEnumerable<CategoryDataDTO>>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        SettingEntity? categoriesSetting = await _settingRepository.GetSettingByType(x => x.Type == request.Type);
        if (categoriesSetting is null)
        {
            return new ApiErrorResult<IEnumerable<CategoryDataDTO>>($"{ErrorMessages.CategorySettingNotFound}", (int)HttpStatusCode.NotFound);
        }

        IEnumerable<CategoryDataDTO> result = JsonConvert.DeserializeObject<IEnumerable<CategoryDataDTO>>(categoriesSetting.Content) ?? [];
        return result is null
            ? new ApiErrorResult<IEnumerable<CategoryDataDTO>>($"{ErrorMessages.CategorySettingNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<IEnumerable<CategoryDataDTO>>(result);
    }
}