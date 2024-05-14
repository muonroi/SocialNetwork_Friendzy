namespace Management.Photo.Infrastructure.Persistences.Query;

public static class CustomSqlQuery
{
    public const string GetImageByName = @"SELECT
                                                sti.Id,
                                                sti.StoreName ,
                                                sti.StoreDescription ,
                                                sti.StoreUrl ,
                                                sti.UserId,
                                                sti.StoreInfoType
                                                FROM StoreInfoEntities sti
                                                WHERE sti.UserId = @userId AND sti.StoreInfoType = @storeInfoType";
}