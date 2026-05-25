using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ListingApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingImages_Listings_ListingId",
                table: "ListingImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingImages",
                table: "ListingImages");

            migrationBuilder.RenameTable(
                name: "ListingImages",
                newName: "ListingImage");

            migrationBuilder.RenameIndex(
                name: "IX_ListingImages_ListingId",
                table: "ListingImage",
                newName: "IX_ListingImage_ListingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingImage",
                table: "ListingImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ListingImage_Listings_ListingId",
                table: "ListingImage",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingImage_Listings_ListingId",
                table: "ListingImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingImage",
                table: "ListingImage");

            migrationBuilder.RenameTable(
                name: "ListingImage",
                newName: "ListingImages");

            migrationBuilder.RenameIndex(
                name: "IX_ListingImage_ListingId",
                table: "ListingImages",
                newName: "IX_ListingImages_ListingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingImages",
                table: "ListingImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ListingImages_Listings_ListingId",
                table: "ListingImages",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
