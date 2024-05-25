#nullable disable

namespace Post.Infrastructure.Persistance.Migrations;

/// <inheritdoc />
public partial class InitDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AlterDatabase()
            .Annotation("MySQL:Charset", "utf8mb4");

        _ = migrationBuilder.CreateTable(
            name: "Posts",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                Content = table.Column<string>(type: "longtext", nullable: false),
                ImageUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                VideoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                AudioUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                FileUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                Slug = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                IsPublished = table.Column<bool>(type: "tinyint(1)", nullable: false),
                IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                CategoryId = table.Column<long>(type: "bigint", nullable: false),
                AuthorId = table.Column<long>(type: "bigint", nullable: false),
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
                _ = table.PrimaryKey("PK_Posts", x => x.Id);
            })
            .Annotation("MySQL:Charset", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "Posts");
    }
}