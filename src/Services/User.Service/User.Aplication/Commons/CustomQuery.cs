namespace User.Application.Commons;

public static class CustomQuery
{
    public const string GetUserByInput = @"
                    WITH UserInfo AS (
                        SELECT
                            u.Id,
                            u.FirstName,
                            u.LastName,
                            u.PhoneNumber,
                            u.EmailAddress,
                            u.AvatarUrl,
                            u.[Address],
                            u.ProfileImagesUrl,
                            u.Longitude,
                            u.Latitude,
                            u.Gender,
                            u.Birthdate,
                            u.CategoryId,
                            u.AccountGuid,
                            u.CreatedDate,
                            u.LastModifiedDate,
                            u.DeletedDate,
                            u.CreatedBy,
                            u.LastModifiedBy,
                            u.DeletedBy,
                            u.CreatedDateTs,
                            u.LastModifiedDateTs
                        FROM Users u
                    ),

                    FilteredUserInfo AS (
                        SELECT * FROM UserInfo WHERE LastName LIKE @Input + '%'
                        UNION ALL
                        SELECT * FROM UserInfo WHERE FirstName LIKE @Input + '%'
                        UNION ALL
                        SELECT * FROM UserInfo WHERE EmailAddress = @Input
                        UNION ALL
                        SELECT * FROM UserInfo WHERE PhoneNumber = @Input
                        UNION ALL
                        SELECT * FROM UserInfo WHERE AccountGuid = TRY_CONVERT(uniqueidentifier, @Input)
                        UNION ALL
                        SELECT * FROM UserInfo WHERE Id = TRY_CONVERT(int, @Input)
                    )

                    SELECT DISTINCT
                        Id,
                        FirstName,
                        LastName,
                        PhoneNumber,
                        EmailAddress,
                        AvatarUrl,
                        [Address],
                        ProfileImagesUrl,
                        Longitude,
                        Latitude,
                        Gender,
                        Birthdate,
                        CategoryId,
                        AccountGuid,
                        CreatedDate,
                        LastModifiedDate,
                        DeletedDate,
                        CreatedBy,
                        LastModifiedBy,
                        DeletedBy,
                        CreatedDateTs,
                        LastModifiedDateTs
                    FROM FilteredUserInfo
                    ORDER BY CreatedDate DESC;";

    public const string GetUsersByInput = @"
                    WITH UserInfo AS (
                        SELECT
                            u.FirstName,
                            u.LastName,
                            u.[Address],
                            u.ProfileImagesUrl,
                            u.Birthdate,
                            u.EmailAddress,
                            u.Gender,
                            u.Id,
                            u.Latitude,
                            u.Longitude,
                            u.AvatarUrl,
                            u.PhoneNumber,
                            u.AccountGuid,
                            u.CategoryId,
                            u.LastModifiedDateTs
                        FROM Users u
                    ),

                    FilteredUserInfo AS (
                        SELECT * FROM UserInfo WHERE PhoneNumber IN (SELECT value FROM STRING_SPLIT(@Input, ','))
                        UNION ALL
                        SELECT * FROM UserInfo WHERE CAST(AccountGuid AS VARCHAR(36)) IN (SELECT value FROM STRING_SPLIT(@Input, ','))
                        UNION ALL
                        SELECT * FROM UserInfo WHERE Id IN (SELECT TRY_CONVERT(BIGINT, value) FROM STRING_SPLIT(@Input, ','))
                    )

                    -- Truy vấn cuối cùng để lấy dữ liệu cần thiết
                    SELECT DISTINCT
                        FirstName,
                        LastName,
                        [Address],
                        ProfileImagesUrl,
                        Birthdate,
                        EmailAddress,
                        Gender,
                        Id,
                        Latitude,
                        Longitude,
                        AvatarUrl,
                        PhoneNumber,
                        AccountGuid,
                        CategoryId,
                        LastModifiedDateTs
                    FROM FilteredUserInfo
                    ORDER BY Id -- Sắp xếp theo Id, có thể thay đổi nếu cần
                                        OFFSET (@PageNumber - 1) * @PageSize ROWS
                                        FETCH NEXT @PageSize ROWS ONLY;
                    ";
}