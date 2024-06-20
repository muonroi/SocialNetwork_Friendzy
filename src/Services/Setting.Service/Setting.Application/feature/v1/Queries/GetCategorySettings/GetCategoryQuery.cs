using Shared.Enums;

namespace Setting.Application.feature.v1.Queries.GetCategorySettings;

public record GetCategoryQuery(SettingsConfig Type) : IRequest<ApiResult<IEnumerable<CategoryDataModel>>>
{
    public SettingsConfig Type { get; set; } = Type;
}