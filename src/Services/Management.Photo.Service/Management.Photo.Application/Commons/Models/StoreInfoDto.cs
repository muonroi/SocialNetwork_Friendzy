namespace Management.Photo.Application.Commons.Models;

public record StoreInfoDTO(string Id, string StoreName, string StoreDescription, string StoreUrl, string Type, string CreatedBy, string CreatedDate, string UpdatedBy, string UpdatedDate, long UserId);