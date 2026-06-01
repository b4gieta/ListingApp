using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ListingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSortOrderToImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ListingImages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ListingImages");
        }
    }
}
