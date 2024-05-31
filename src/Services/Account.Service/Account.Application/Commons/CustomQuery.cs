namespace Account.Application.Commons
{
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
                                                    a.Status
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
                                                    a.Status;
                                                ";

    }
}
