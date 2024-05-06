namespace Setting.Application.feature.v1.Queries.GetCategorySettings;

public class GetCategoryQueryHandler(ISettingRepository<SettingEntity, long> settingRepository) :
    IRequestHandler<GetCategoryQuery, ApiResult<IEnumerable<CategoryData>>>
{
    private readonly ISettingRepository<SettingEntity, long> _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));

    public async Task<ApiResult<IEnumerable<CategoryData>>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        SettingEntity? categoriesSetting = await _settingRepository.GetSettingByType(x => x.Type == request.Type);
        if (categoriesSetting is null)
        {
            return new ApiErrorResult<IEnumerable<CategoryData>>($"{ErrorMessages.CategorySettingNotFound}", (int)HttpStatusCode.NotFound);
        }

        IEnumerable<CategoryData> result = JsonConvert.DeserializeObject<IEnumerable<CategoryData>>(categoriesSetting.Content) ?? [];
        return result is null
            ? new ApiErrorResult<IEnumerable<CategoryData>>($"{ErrorMessages.CategorySettingNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<IEnumerable<CategoryData>>(result);
    }
}