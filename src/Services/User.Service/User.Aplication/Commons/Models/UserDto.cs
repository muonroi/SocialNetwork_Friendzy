using User.Application.Commons.Mappings;
namespace User.Application.Commons.Models;

public record UserDto : IMapFrom<UserEntity>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public IEnumerable<string> ProfileImages { get; set; } = [];
    public double Longtitude { get; set; }
    public double Latitude { get; set; }
    public Gender Gender { get; set; }
    public long? Birthdate { get; set; }
    public Guid AccountGuid { get; set; }

}