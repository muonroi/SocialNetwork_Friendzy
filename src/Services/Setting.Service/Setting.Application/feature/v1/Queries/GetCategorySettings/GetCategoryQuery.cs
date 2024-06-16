using ExternalAPI.Models;

namespace Setting.Application.feature.v1.Queries.GetCategorySettings;

public class GetCategoryQuery(Settings type) : IRequest<ApiResult<IEnumerable<CategoryDataModel>>>
{
    public Settings Type { get; set; } = type.CompareTo(Settings.None) == 0 ? throw new ArgumentNullException(nameof(type)) : type;
}