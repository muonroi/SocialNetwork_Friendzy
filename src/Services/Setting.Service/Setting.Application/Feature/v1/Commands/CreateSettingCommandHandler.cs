namespace Setting.Application.feature.v1.Commands;

public class CreateSettingCommandHandler(ISettingRepository<SettingEntity, long> settingRepository) :
IRequestHandler<CreateSettingCommand, ApiResult<UserOnlineModel>>
{
    private readonly ISettingRepository<SettingEntity, long> _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));

    public async Task<ApiResult<UserOnlineModel>> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
    {
        SettingEntity? categoriesSetting = await _settingRepository.GetSettingByType(x => x.Type == request.Type);

        if (categoriesSetting is null)
        {
            await _settingRepository.CreateSettingByType(new SettingEntity
            {
                Name = request.Name,
                Description = request.Description,
                Content = request.Content,
                Type = request.Type
            }, cancellationToken);
            _ = await _settingRepository.SaveChangesAsync();

            return new ApiSuccessResult<UserOnlineModel>(new UserOnlineModel());
        }
        categoriesSetting.Name = request.Name;
        categoriesSetting.Description = request.Description;
        categoriesSetting.Content = request.Content;
        categoriesSetting.Type = request.Type;
        await _settingRepository.UpdateAsync(categoriesSetting);
        _ = await _settingRepository.SaveChangesAsync();

        return new ApiSuccessResult<UserOnlineModel>(new UserOnlineModel());
    }
}
