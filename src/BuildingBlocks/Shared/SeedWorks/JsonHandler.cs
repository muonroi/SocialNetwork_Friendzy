namespace Shared.SeedWorks;

public static class JsonHandler
{
    public static string GetValue(this string key, Languages languages, params object[] arguments)
    {
        string? executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string resourcePath = Path.Combine(executableLocation!, "Resources", $"ErrorMessages-{languages}.json");
        string json = File.ReadAllText(resourcePath);
        Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json) ?? throw new InvalidOperationException("Failed to deserialize JSON data");
        string resultMessage = data.TryGetValue(key, out object? value) ? (string)value : (string)data["ErrorUnknown"];
        return arguments.Length != 0 ? string.Format(resultMessage, arguments) : resultMessage;
    }
}