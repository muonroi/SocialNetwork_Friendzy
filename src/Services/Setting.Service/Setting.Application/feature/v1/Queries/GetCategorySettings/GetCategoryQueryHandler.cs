using ExternalAPI.Models;

namespace Setting.Application.feature.v1.Queries.GetCategorySettings;

public class GetCategoryQueryHandler(ISettingRepository<SettingEntity, long> settingRepository,
    ISerializeService serializeService) :
    IRequestHandler<GetCategoryQuery, ApiResult<IEnumerable<CategoryDataModel>>>
{
    private readonly ISettingRepository<SettingEntity, long> _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));

    private readonly ISerializeService _serializeService = serializeService ?? throw new ArgumentNullException(nameof(serializeService));

    public async Task<ApiResult<IEnumerable<CategoryDataModel>>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        SettingEntity? categoriesSetting = await _settingRepository.GetSettingByType(x => x.Type == request.Type);
        if (categoriesSetting is null)
        {
            return new ApiErrorResult<IEnumerable<CategoryDataModel>>($"{SettingErrorMessages.CategorySettingNotFound}", (int)HttpStatusCode.NotFound);
        }

        IEnumerable<CategoryDataModel> result = _serializeService.Deserialize<IEnumerable<CategoryDataModel>>(categoriesSetting.Content) ?? [];
        return result is null
            ? new ApiErrorResult<IEnumerable<CategoryDataModel>>($"{SettingErrorMessages.CategorySettingNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<IEnumerable<CategoryDataModel>>(result);
    }
}