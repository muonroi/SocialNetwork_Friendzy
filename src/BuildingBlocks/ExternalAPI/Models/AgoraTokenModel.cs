namespace ExternalAPI.Models
{
    public record AgoraTokenModel
    {
        [JsonProperty("key")]
        public string Key { get; init; } = string.Empty;
    }
}