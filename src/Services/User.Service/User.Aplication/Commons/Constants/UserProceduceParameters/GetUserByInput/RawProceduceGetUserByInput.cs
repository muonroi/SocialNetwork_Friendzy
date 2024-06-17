namespace User.Application.Commons.Constants.UserProceduceParameters.GetUserByInput;

public class RawProceduceGetUserByInput
{
    public const string query = @"
CREATE PROC GetUserByInput
                                @Input varchar(50)
                                AS
                                BEGIN
                                    -- Tạo bảng tạm để lưu kết quả tìm kiếm
                                    CREATE TABLE #TempUserResult (
                                        FirstName nvarchar(255),
                                        LastName nvarchar(255),
                                        [Address] nvarchar(max),
                                        ProfileImagesUrl varchar(max),
                                        Birthdate bigint,
                                        EmailAddress varchar(100),
                                        Gender int,
                                        Id bigint,
                                        Latitude decimal(10, 6),
                                        Longitude decimal(10, 6),
                                        AvatarUrl varchar(max),
                                        PhoneNumber varchar(20),
                                        AccountGuid uniqueidentifier,
                                        CategoryId varchar(255),
                                    )

                                    -- Chèn dữ liệu từ bảng Users vào bảng tạm
                                    INSERT INTO #TempUserResult
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
                                		u.CategoryId
                                    FROM Users u

                                    -- Truy vấn các bản ghi từ bảng tạm theo nhiều điều kiện
                                    SELECT * FROM #TempUserResult WHERE LastName LIKE @Input + '%'
                                    UNION ALL
                                    SELECT * FROM #TempUserResult WHERE FirstName LIKE @Input + '%'
                                    UNION ALL
                                    SELECT * FROM #TempUserResult WHERE EmailAddress = @Input
                                    UNION ALL
                                    SELECT * FROM #TempUserResult WHERE PhoneNumber = @Input
                                    UNION ALL
                                    SELECT * FROM #TempUserResult WHERE Id = TRY_CONVERT(int, @Input)

                                    -- Xóa bảng tạm
                                    DROP TABLE #TempUserResult
                                END
";
}