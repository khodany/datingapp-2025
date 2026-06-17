using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class photsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_photos_Members_MemberId",
                table: "photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_photos",
                table: "photos");

            migrationBuilder.RenameTable(
                name: "photos",
                newName: "Photoshotos");

            migrationBuilder.RenameIndex(
                name: "IX_photos_MemberId",
                table: "Photoshotos",
                newName: "IX_Photoshotos_MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photoshotos",
                table: "Photoshotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photoshotos_Members_MemberId",
                table: "Photoshotos",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photoshotos_Members_MemberId",
                table: "Photoshotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photoshotos",
                table: "Photoshotos");

            migrationBuilder.RenameTable(
                name: "Photoshotos",
                newName: "photos");

            migrationBuilder.RenameIndex(
                name: "IX_Photoshotos_MemberId",
                table: "photos",
                newName: "IX_photos_MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_photos",
                table: "photos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_photos_Members_MemberId",
                table: "photos",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
