namespace Setting.Application.feature.v1.Queries.GetUserOnlineSettings;

public class GetUserOnlineQueryHandler(ISettingRepository<SettingEntity, long> settingRepository,
    ISerializeService serializeService) :
    IRequestHandler<GetUserOnlineQuery, ApiResult<IEnumerable<UserOnlineModel>>>
{
    private readonly ISettingRepository<SettingEntity, long> _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));

    private readonly ISerializeService _serializeService = serializeService ?? throw new ArgumentNullException(nameof(serializeService));

    public async Task<ApiResult<IEnumerable<UserOnlineModel>>> Handle(GetUserOnlineQuery request, CancellationToken cancellationToken)
    {
        SettingEntity? userOnlineSetting = await _settingRepository.GetSettingByType(x => x.Type == request.Type);
        if (userOnlineSetting is null)
        {
            return new ApiErrorResult<IEnumerable<UserOnlineModel>>($"{SettingErrorMessages.CategorySettingNotFound}", (int)HttpStatusCode.NotFound);
        }

        IEnumerable<UserOnlineModel>? result = _serializeService.Deserialize<IEnumerable<UserOnlineModel>>(userOnlineSetting.Content) ?? [];

        return !result.Any()
            ? new ApiErrorResult<IEnumerable<UserOnlineModel>>($"{SettingErrorMessages.CategorySettingNotFound}", (int)HttpStatusCode.NotFound)
            : new ApiSuccessResult<IEnumerable<UserOnlineModel>>(result);
    }
}