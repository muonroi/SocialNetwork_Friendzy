namespace Message.Infrastructure.Dtos
{
    public record AuthorDto
    {
        public required string Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DisplayName => $"{FirstName} {LastName}";
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime? LastActive { get; set; }
    }
}
