namespace User.Domain.Entities;

public class UserEntity : EntityAuditBase<long>
{
    [Required]
    [MaxLength(50)]
    [MinLength(2)]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [MinLength(2)]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [MinLength(10)]
    public string PhoneNumber { get; set; } = string.Empty;

    [EmailAddress]
    [Column(TypeName = "nvarchar(255)")]
    public string EmailAddress { get; set; } = string.Empty;

    [MaxLength(1000)]
    [Required]
    public string AvatarUrl { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Address { get; set; } = string.Empty;

    public string ProfileImagesUrl { get; set; } = string.Empty;

    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public Gender Gender { get; set; }

    public long Birthdate { get; set; }

    public string CategoryId { get; set; } = string.Empty;

    public Guid AccountGuid { get; set; }
}