namespace ExternalAPI.DTOs;

public record UserDataDTO
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonProperty("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonProperty("phoneNumber")]
    public long PhoneNumber { get; set; }

    [JsonProperty("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonProperty("avatarUrl")]
    public string AvatarUrl { get; set; } = string.Empty;

    [JsonProperty("address")]
    public string Address { get; set; } = string.Empty;

    [JsonProperty("profileImages")]
    public string[] ProfileImages { get; set; } = [];

    [JsonProperty("longtitude")]
    public long Longtitude { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("gender")]
    public long Gender { get; set; }

    [JsonProperty("birthdate")]
    public DateTime Birthdate { get; set; }

    [JsonProperty("accountGuid")]
    public Guid AccountGuid { get; set; }
}