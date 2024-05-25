using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace Account.Infrastructure.Persistance
{
    public class AccountDbContextSeed
    {
        private readonly ILogger _logger;
        private readonly AccountDbContext _context;

        public AccountDbContextSeed(ILogger logger, AccountDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            // Seed accounts
            if (!_context.Accounts.Any())
            {
                string accountsJson = @"[
                    {
                        ""Id"": ""123e4567-e89b-12d3-a456-426614174000"",
                        ""AccountType"": ""Personal"",
                        ""Currency"": ""USD"",
                        ""LockReason"": ""Fraudulent activity detected"",
                        ""Balance"": 1000.00,
                        ""IsActive"": false,
                        ""IsEmailVerified"": true,
                        ""Status"": ""Online""
                    },
                    {
                        ""Id"": ""223e4567-e89b-12d3-a456-426614174001"",
                        ""AccountType"": ""Business"",
                        ""Currency"": ""EUR"",
                        ""LockReason"": """",
                        ""Balance"": 2500.00,
                        ""IsActive"": true,
                        ""IsEmailVerified"": true,
                        ""Status"": ""Offline""
                    },
                    {
                        ""Id"": ""323e4567-e89b-12d3-a456-426614174002"",
                        ""AccountType"": ""Personal"",
                        ""Currency"": ""USD"",
                        ""LockReason"": """",
                        ""Balance"": 50000.00,
                        ""IsActive"": true,
                        ""IsEmailVerified"": true,
                        ""Status"": ""Offline""
                    },
                    {
                        ""Id"": ""423e4567-e89b-12d3-a456-426614174003"",
                        ""AccountType"": ""Business"",
                        ""Currency"": ""USD"",
                        ""LockReason"": """",
                        ""Balance"": 15000.00,
                        ""IsActive"": true,
                        ""IsEmailVerified"": false,
                        ""Status"": ""Online""
                    },
                    {
                        ""Id"": ""523e4567-e89b-12d3-a456-426614174004"",
                        ""AccountType"": ""Personal"",
                        ""Currency"": ""EUR"",
                        ""LockReason"": ""Account under investigation"",
                        ""Balance"": 0.00,
                        ""IsActive"": false,
                        ""IsEmailVerified"": false,
                        ""Status"": ""Busy""
                    }
                ]";

                List<AccountEntity>? accounts = JsonConvert.DeserializeObject<List<AccountEntity>>(accountsJson);
                if (accounts != null)
                {
                    await _context.Accounts.AddRangeAsync(accounts);
                    _ = await _context.SaveChangesAsync(); // Ensure account IDs are generated
                    _logger.Information("Seeded Accounts: {@Accounts}", accounts);
                }
            }

            // Seed roles
            if (!_context.RoleEntities.Any())
            {
                List<RoleEntity> roles =
                [
                    new RoleEntity { Name = "Admin" },
                    new RoleEntity { Name = "User" },
                    new RoleEntity { Name = "Guest" }
                ];

                await _context.RoleEntities.AddRangeAsync(roles);
                _ = await _context.SaveChangesAsync(); // Ensure role IDs are generated
                _logger.Information("Seeded Roles: {@Roles}", roles);
            }

            // Seed account roles
            if (!_context.AccountRoles.Any() && _context.Accounts.Any() && _context.RoleEntities.Any())
            {
                List<AccountEntity> accounts = await _context.Accounts.ToListAsync();
                List<RoleEntity> roles = await _context.RoleEntities.ToListAsync();

                RoleEntity adminRole = roles.First(r => r.Name == "Admin");
                RoleEntity userRole = roles.First(r => r.Name == "User");

                List<AccountRolesEntity> accountRoles =
                [
                    new AccountRolesEntity { AccountId = accounts[0].Id, RoleId = adminRole.Id },
                    new AccountRolesEntity { AccountId = accounts[1].Id, RoleId = userRole.Id }
                ];

                await _context.AccountRoles.AddRangeAsync(accountRoles);
                _ = await _context.SaveChangesAsync();
                _logger.Information("Seeded AccountRoles: {@AccountRoles}", accountRoles);
            }
        }
    }
}
