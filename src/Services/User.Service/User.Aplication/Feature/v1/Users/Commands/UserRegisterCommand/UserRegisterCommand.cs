namespace User.Application.Feature.v1.Users.Commands.UserRegisterCommand
{
    public record UserRegisterCommand : IRequest<ApiResult<UserDto>>
    {
        public required string PhoneNumber { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? EmailAddress { get; set; }

        public required string AvatarUrl { get; set; }

        public required string Address { get; set; }

        public required string ProfileImagesUrl { get; set; }

        public required double Longitude { get; set; }

        public required double Latitude { get; set; }

        public required Gender Gender { get; set; }

        public required long Birthdate { get; set; }

        public required string CategoryId { get; set; }

    }
}
