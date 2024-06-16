namespace ExternalAPI.Models;

public record ExternalApiResponse<T>
{
    [JsonProperty("isSucceeded")]
    public bool IsSucceeded { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    [JsonProperty("statusCode")]
    public long StatusCode { get; set; }

    [JsonProperty("data")]
    public required T Data { get; set; }
}
