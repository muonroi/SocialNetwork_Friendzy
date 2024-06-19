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
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonProperty("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonProperty("avatarUrl")]
    public string AvatarUrl { get; set; } = string.Empty;

    [JsonProperty("address")]
    public string Address { get; set; } = string.Empty;

    [JsonProperty("profileImages")]
    public string[] ProfileImages { get; set; } = [];

    [JsonProperty("longitude")]
    public double Longitude { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("gender")]
    public int Gender { get; set; }

    [JsonProperty("birthDate")]
    public DateTime BirthDate { get; set; }

    [JsonProperty("accountGuid")]
    public Guid AccountGuid { get; set; }

    [JsonProperty("matchScore")]
    public double MatchScore { get; set; }

    [JsonProperty("lastModifiedDateTs")]
    public DateTime LastModifiedDateTs { get; set; }
}