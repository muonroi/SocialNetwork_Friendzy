namespace ExternalAPI.Models;

public class UserOnlineModel
{
    public string Key { get; set; } = string.Empty;

    public IEnumerable<string> Value { get; set; } = [];
}
