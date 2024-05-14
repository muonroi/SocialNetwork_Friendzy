namespace ExternalAPI.DTOs;

public class CategoryDTO
{
    [JsonProperty("isSucceeded")]
    public bool IsSucceeded { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    [JsonProperty("statusCode")]
    public long StatusCode { get; set; }

    [JsonProperty("data")]
    public IEnumerable<CategoryData> Data { get; set; } = [];
}