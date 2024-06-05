#nullable disable

namespace Account.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddRefreshTokenColumn : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "RefreshToken",
            table: "Accounts",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<double>(
            name: "RefreshTokenExpiryTime",
            table: "Accounts",
            type: "float",
            nullable: false,
            defaultValue: 0.0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "RefreshToken",
            table: "Accounts");

        migrationBuilder.DropColumn(
            name: "RefreshTokenExpiryTime",
            table: "Accounts");
    }
}