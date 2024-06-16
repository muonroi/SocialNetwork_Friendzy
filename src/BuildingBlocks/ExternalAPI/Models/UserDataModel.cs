namespace ExternalAPI.Models;

public record UserDataModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("fullName")]
    public string FullName => $"{FirstName} {LastName}";

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

    [JsonProperty("longitude")]
    public long Longitude { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("gender")]
    public long Gender { get; set; }

    [JsonProperty("birthdate")]
    public DateTime Birthdate { get; set; }

    [JsonProperty("accountGuid")]
    public Guid AccountGuid { get; set; }

    [JsonProperty("matchScore")]
    public double MatchScore { get; set; }
}