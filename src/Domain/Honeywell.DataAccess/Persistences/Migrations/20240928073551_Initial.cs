using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Honeywell.DataAccess.Persistences.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileSizeInMb = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoFiles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VideoFiles",
                columns: new[] { "Id", "FileName", "FilePath", "FileSizeInMb", "FileType", "UploadedOn" },
                values: new object[] { 1, "big_buck_bunny.mp4", "videos/big_buck_bunny.mp4", 5510872m, "video/mp4", new DateTime(2024, 9, 28, 7, 35, 51, 233, DateTimeKind.Utc).AddTicks(150) });

            migrationBuilder.CreateIndex(
                name: "IX_VideoFiles_FileName",
                table: "VideoFiles",
                column: "FileName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoFiles");
        }
    }
}
