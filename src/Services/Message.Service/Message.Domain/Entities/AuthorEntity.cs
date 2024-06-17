namespace Message.Domain.Entities;

public class AuthorEntity : EntityAuditBase<string>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    public AuthorEntity()
    { }

    public AuthorEntity(string firstName, string lastName, string imageUrl)
    {
        FirstName = firstName;
        LastName = lastName;
        ImageUrl = imageUrl;
    }
}