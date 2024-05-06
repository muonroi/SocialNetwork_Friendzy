using Contracts.Domains;
using Shared.Enums;

namespace Management.Photo.Domain.Entities;

public class StoreInfoEntity : EntityAuditBase<long>
{
    public string StoreName { get; set; } = string.Empty;

    public long UserId { get; set; }

    public string StoreDescription { get; set; } = string.Empty;

    public string StoreUrl { get; set; } = string.Empty;

    public StoreInfoType StoreInfoType { get; set; }
}