using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Management.Friends.Action.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            _ = migrationBuilder.CreateTable(
                name: "FriendsActions",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    FriendId = table.Column<long>(type: "bigint", nullable: false),
                    ActionMatched = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true),
                    CreatedDateTs = table.Column<long>(type: "bigint", nullable: true),
                    LastModifiedDateTs = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_FriendsActions", x => new { x.FriendId, x.UserId });
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "FriendsActions");
        }
    }
}
