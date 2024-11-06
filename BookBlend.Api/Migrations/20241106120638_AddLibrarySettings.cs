using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBlend.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLibrarySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LibrarySettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DefaultLanguage = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibrarySettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryPaths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    LibrarySettingsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LibrarySettingsId1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryPaths_LibrarySettings_LibrarySettingsId",
                        column: x => x.LibrarySettingsId,
                        principalTable: "LibrarySettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryPaths_LibrarySettings_LibrarySettingsId1",
                        column: x => x.LibrarySettingsId1,
                        principalTable: "LibrarySettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryPaths_LibrarySettingsId",
                table: "LibraryPaths",
                column: "LibrarySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryPaths_LibrarySettingsId1",
                table: "LibraryPaths",
                column: "LibrarySettingsId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryPaths");

            migrationBuilder.DropTable(
                name: "LibrarySettings");
        }
    }
}
