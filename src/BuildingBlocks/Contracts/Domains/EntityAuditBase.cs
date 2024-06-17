namespace Contracts.Domains;

public abstract class EntityAuditBase<T> : EntityBase<T>, IAuditable
{
    // Ngày tạo entity
    [BsonElement("createdDate")]
    public DateTimeOffset CreatedDate { get; set; }

    // Ngày sửa đổi cuối cùng của entity
    [BsonElement("lastModifiedDate")]
    public DateTimeOffset? LastModifiedDate { get; set; }

    // Ngày xóa entity
    [BsonElement("deletedDate")]
    public DateTimeOffset? DeletedDate { get; set; }

    // Người tạo entity
    [BsonElement("createdBy")]
    public string? CreatedBy { get; set; }

    // Người sửa đổi cuối cùng của entity
    [BsonElement("lastModifiedBy")]
    public string? LastModifiedBy { get; set; }

    // Người xóa entity
    [BsonElement("deletedBy")]
    public string? DeletedBy { get; set; }

    // Timestamp của ngày tạo
    [BsonElement("createdDateTs")]
    public long? CreatedDateTs { get; set; }

    // Timestamp của ngày sửa đổi cuối cùng
    [BsonElement("lastModifiedDateTs")]
    public long? LastModifiedDateTs { get; set; }
}