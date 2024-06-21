namespace ExternalAPI.Models;

public class SettingRequestModel
{
    public SettingsConfig Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
