using Newtonsoft.Json;

namespace ExternalAPI.DTOs
{
    public class MultipleUsersDto
    {
        [JsonProperty("isSucceeded")]
        public bool IsSucceeded { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("data")]
        public IEnumerable<UserDTOData> Data { get; set; } = [];
    }
}