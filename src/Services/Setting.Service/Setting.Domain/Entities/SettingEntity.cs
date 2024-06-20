using Shared.Enums;

namespace Setting.Domain.Entities;

public class SettingEntity : EntityAuditBase<long>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public SettingsConfig Type { get; set; }
}