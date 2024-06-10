namespace User.Application.Commons.Constants.UserProceduceParameters.GetUsersByInput;

public class RawProceduceGetUsersByInput
{
    public const string query = @"                    
                                    CREATE PROC GetUsersByInput
                                    @Input varchar(50),
                                    @PageNumber int,
                                    @PageSize int
                                    AS
                                    BEGIN
                                        -- Tạo bảng tạm để lưu kết quả tìm kiếm
                                        CREATE TABLE #TempUserResult (
                                            FirstName varchar(50),
                                            LastName varchar(50),
                                            [Address] varchar(255),
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
                                    		CategoryId varchar(255)
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
                                    
                                        -- Số lượng bản ghi bỏ qua
                                        DECLARE @Offset int = (@PageNumber - 1) * @PageSize
                                    
                                        -- Truy vấn các bản ghi từ bảng tạm theo điều kiện PhoneNumber và phân trang
                                        SELECT * FROM #TempUserResult
                                        WHERE PhoneNumber IN (SELECT value FROM STRING_SPLIT(@Input, ','))
                                        UNION ALL
                                        -- Truy vấn các bản ghi từ bảng tạm theo điều kiện Id và phân trang
                                        SELECT * FROM #TempUserResult
                                        WHERE Id IN (SELECT value FROM STRING_SPLIT(@Input, ','))
                                        ORDER BY Id -- Thứ tự sắp xếp, có thể thay đổi theo yêu cầu
                                        OFFSET @Offset ROWS
                                        FETCH NEXT @PageSize ROWS ONLY
                                    
                                        -- Xóa bảng tạm
                                        DROP TABLE #TempUserResult
                                    END
                                    ";
}