namespace Setting.Application.feature.v1.Commands;

public class CreateSettingCommand : IRequest<ApiResult<UserOnlineModel>>
{
    public int Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
