namespace User.Application.Feature.v1.Users.Commands.UserUpdateInfoCommand;

public class UserUpdateInfoCommand : IRequest<ApiResult<UserDto>>
{
    public string PhoneNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? EmailAddress { get; set; }

    public string AvatarUrl { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string ProfileImagesUrl { get; set; } = string.Empty;

    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public Gender Gender { get; set; }

    public long BirthDate { get; set; }

    public string CategoryId { get; set; } = string.Empty;
}