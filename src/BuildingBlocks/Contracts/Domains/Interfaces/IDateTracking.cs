namespace Contracts.Domains.Interfaces;

public interface IDateTracking
{
    DateTimeOffset CreatedDate { get; set; }

    DateTimeOffset? LastModifiedDate { get; set; }

    long? CreatedDateTs { get; set; }

    long? LastModifiedDateTs { get; set; }
}