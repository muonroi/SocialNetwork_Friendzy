namespace Message.Domain.Entities
{
    public class AuthorEntity(string firstName, string id, string lastName, string imageUrl) :
    {
        public string FirstName { get; set; } = firstName;
        public string Id { get; set; } = id;
        public string LastName { get; set; } = lastName;
        public string ImageUrl { get; set; } = imageUrl;
    }
}
