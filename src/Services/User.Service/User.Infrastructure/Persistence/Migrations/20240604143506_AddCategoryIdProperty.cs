#nullable disable

namespace User.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddCategoryIdProperty : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Longtitude",
            table: "Users",
            newName: "Longitude");

        migrationBuilder.AddColumn<string>(
            name: "CategoryId",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CategoryId",
            table: "Users");

        migrationBuilder.RenameColumn(
            name: "Longitude",
            table: "Users",
            newName: "Longtitude");
    }
}