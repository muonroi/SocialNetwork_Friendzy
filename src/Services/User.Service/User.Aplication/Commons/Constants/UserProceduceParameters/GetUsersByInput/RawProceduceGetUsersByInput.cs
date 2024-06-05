namespace User.Application.Commons.Constants.UserProceduceParameters.GetUsersByInput;

public class RawProceduceGetUsersByInput
{
    public const string query = @"CREATE PROC GetUsersByInput
                                    @Input varchar(50)
                                    AS
                                    BEGIN
                                        -- Tạo bảng tạm để lưu kết quả tìm kiếm
                                        CREATE TABLE #TempUserResult (
                                            FirstName varchar(50),
                                            LastName varchar(50),
                                            [Address] varchar(255),
                                            ProfileImagesUrl varchar(255),
                                            Birthdate bigint,
                                            EmailAddress varchar(100),
                                            Gender int,
                                            Id bigint,
                                            Latitude decimal(10, 6),
                                            Longtitude decimal(10, 6),
                                            AvatarUrl varchar(255),
                                            PhoneNumber varchar(20),
                                            AccountGuid uniqueidentifier
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
                                            u.Longtitude,
                                            u.AvatarUrl,
                                            u.PhoneNumber,
                                            u.AccountGuid
                                        FROM Users u

                                        -- Truy vấn các bản ghi từ bảng tạm theo điều kiện PhoneNumber
                                        SELECT * FROM #TempUserResult
                                        WHERE PhoneNumber IN (SELECT value FROM STRING_SPLIT(@Input, ','))

                                        UNION ALL

                                        -- Truy vấn các bản ghi từ bảng tạm theo điều kiện Id
                                        SELECT * FROM #TempUserResult
                                        WHERE Id IN (SELECT value FROM STRING_SPLIT(@Input, ','))

                                        -- Xóa bảng tạm
                                        DROP TABLE #TempUserResult
                                    END
                                    ";
}