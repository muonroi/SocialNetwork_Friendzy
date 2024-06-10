namespace Management.Photo.Infrastructure.Persistence.Query;

public static class CustomSqlQuery
{
    public const string GetImageByType = @"
                                            SELECT
                                                sti.Id,
                                                sti.StoreName,
                                                sti.StoreDescription,
                                                sti.StoreUrl,
                                                sti.Index,
                                                sti.UserId,
                                                sti.StoreInfoType,
                                                be.BucketName,
                                                be.BucketDescription
                                            FROM
                                                StoreInfoEntities sti
                                            LEFT JOIN
                                                BucketEntities be
                                            ON
                                                sti.BucketId = be.Id
                                            WHERE
                                                sti.UserId = @userId
                                                AND be.Id =  @bucketId
                                                AND sti.StoreInfoType = @storeInfoType
                                        ";

    public const string GetImageById = @"

                                        SELECT
                                            sti.Id,
                                            sti.StoreName,
                                            sti.StoreDescription,
                                            sti.StoreUrl,
                                            sti.Index,
                                            sti.UserId,
                                            sti.StoreInfoType,
                                            be.BucketName,
                                            be.BucketDescription
                                        FROM
                                            StoreInfoEntities sti
                                        LEFT JOIN
                                            BucketEntities be
                                        ON
                                            sti.BucketId = be.Id
                                        WHERE
                                            sti.UserId = @userId
                                            AND sti.Id = @storyInfoId
                                            AND be.Id =  @bucketId
                                        ";

    public const string GetBuckets = @"SELECT be.Id, be.BucketName, be.BucketDescription FROM BucketEntities be";

    public const string GetBucketById = @"SELECT be.Id, be.BucketName, be.BucketDescription FROM BucketEntities be WHERE be.Id = @id";
}