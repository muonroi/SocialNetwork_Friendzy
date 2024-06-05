#nullable disable

namespace Account.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class InitDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Accounts",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AccountType = table.Column<int>(type: "int", nullable: false),
                Currency = table.Column<int>(type: "int", nullable: false),
                LockReason = table.Column<string>(type: "nvarchar(256)", nullable: false),
                Balance = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedDateTs = table.Column<long>(type: "bigint", nullable: true),
                LastModifiedDateTs = table.Column<long>(type: "bigint", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Accounts", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RoleEntities",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedDateTs = table.Column<long>(type: "bigint", nullable: true),
                LastModifiedDateTs = table.Column<long>(type: "bigint", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleEntities", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AccountRoles",
            columns: table => new
            {
                AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedDateTs = table.Column<long>(type: "bigint", nullable: true),
                LastModifiedDateTs = table.Column<long>(type: "bigint", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AccountRoles", x => new { x.AccountId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AccountRoles_Accounts_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AccountRoles_RoleEntities_RoleId",
                    column: x => x.RoleId,
                    principalTable: "RoleEntities",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AccountRoles_RoleId",
            table: "AccountRoles",
            column: "RoleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AccountRoles");

        migrationBuilder.DropTable(
            name: "Accounts");

        migrationBuilder.DropTable(
            name: "RoleEntities");
    }
}