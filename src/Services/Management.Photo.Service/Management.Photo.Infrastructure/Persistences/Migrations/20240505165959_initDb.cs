﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Management.Photo.Infrastructure.Persistences.Migrations;

/// <inheritdoc />
public partial class initDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "StoreInfoEntities",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StoreName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                UserId = table.Column<long>(type: "bigint", nullable: false),
                StoreDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                StoreUrl = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                StoreInfoType = table.Column<int>(type: "int", nullable: false),
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
                table.PrimaryKey("PK_StoreInfoEntities", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "StoreInfoEntities");
    }
}
