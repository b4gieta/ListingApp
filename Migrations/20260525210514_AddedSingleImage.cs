using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ListingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedSingleImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingImage");

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Listings",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Listings");

            migrationBuilder.CreateTable(
                name: "ListingImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ListingId = table.Column<int>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingImage_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingImage_ListingId",
                table: "ListingImage",
                column: "ListingId");
        }
    }
}
