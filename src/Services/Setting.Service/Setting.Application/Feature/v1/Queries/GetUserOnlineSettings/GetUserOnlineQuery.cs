

namespace Setting.Application.feature.v1.Queries.GetUserOnlineSettings;

public record GetUserOnlineQuery(SettingsConfig Type) : IRequest<ApiResult<IEnumerable<UserOnlineModel>>>
{
    public SettingsConfig Type { get; set; } = Type;
}