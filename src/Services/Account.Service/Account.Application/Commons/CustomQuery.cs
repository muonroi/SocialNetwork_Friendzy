namespace Account.Application.Commons;

public static class CustomQuery
{
    public const string GetAccountsPaging = @"

                                                SELECT
                                                    a.Id,
                                                    a.AccountType,
                                                    a.Currency,
                                                    a.LockReason,
                                                    a.Balance,
                                                    a.IsActive,
                                                    a.IsEmailVerified,
                                                    a.Status,
                                                    a.LastModifiedDate,
                                                    STRING_AGG(r.Name, ', ') AS Roles
                                                FROM Accounts a
                                                JOIN AccountRoles ar ON a.Id = ar.AccountId
                                                JOIN RoleEntities r ON ar.RoleId = r.Id
                                                GROUP BY
                                                    a.Id,
                                                    a.AccountType,
                                                    a.Currency,
                                                    a.LockReason,
                                                    a.Balance,
                                                    a.IsActive,
                                                    a.IsEmailVerified,
                                                    a.Status,
                                                    a.LastModifiedDate
                                                ORDER BY a.Id
                                                OFFSET @Offset ROWS
                                                FETCH NEXT @PageSize ROWS ONLY;

                                                ";

    public const string GetAccount = @"

                                                SELECT
                                                    a.Id,
                                                    a.AccountType,
                                                    a.Currency,
                                                    a.LockReason,
                                                    a.Balance,
                                                    a.IsActive,
                                                    a.IsEmailVerified,
                                                    a.Status,
                                                    a.LastModifiedDate,
                                                    STRING_AGG(r.Name, ', ') AS Roles
                                                FROM Accounts a
                                                JOIN AccountRoles ar ON a.Id = ar.AccountId
                                                JOIN RoleEntities r ON ar.RoleId = r.Id
                                               WHERE a.Id = @Id
                                                GROUP BY
                                                    a.Id,
                                                    a.AccountType,
                                                    a.Currency,
                                                    a.LockReason,
                                                    a.Balance,
                                                    a.IsActive,
                                                    a.IsEmailVerified,
                                                    a.Status,
                                                    a.LastModifiedDate;
                                                ";

    public const string GetAccounts = @"

                                                SELECT
                                                    a.Id,
                                                    a.AccountType,
                                                    a.Currency,
                                                    a.LockReason,
                                                    a.Balance,
                                                    a.IsActive,
                                                    a.IsEmailVerified,
                                                    a.Status,
                                                    a.LastModifiedDate,
                                                    STRING_AGG(r.Name, ', ') AS Roles
                                                FROM Accounts a
                                                JOIN AccountRoles ar ON a.Id = ar.AccountId
                                                JOIN RoleEntities r ON ar.RoleId = r.Id
                                               WHERE a.Id IN @Id
                                                GROUP BY
                                                    a.Id,
                                                    a.AccountType,
                                                    a.Currency,
                                                    a.LockReason,
                                                    a.Balance,
                                                    a.IsActive,
                                                    a.IsEmailVerified,
                                                    a.Status,
                                                    a.LastModifiedDate;
                                                ";

    public const string GetAllRoles = @"       SELECT
                                                    r.Id AS RoleId,
                                                    r.Name AS RoleName
                                                FROM RoleEntities r ";

    public const string GetRoleByRoleName = @"       SELECT
                                                    r.Id AS RoleId,
                                                    r.Name AS RoleName
                                                FROM RoleEntities r
                                                WHERE r.Name = @roleName";
}