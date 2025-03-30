using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoritePlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoritePlaces",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    PlaceName = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritePlaces", x => new { x.UserId, x.PlaceName });
                    table.ForeignKey(
                        name: "FK_FavoritePlaces_Places_PlaceName",
                        column: x => x.PlaceName,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoritePlaces_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.UpdateData(
            //    table: "User",
            //    keyColumn: "Id",
            //    keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
            //    column: "PasswordHash",
            //    value: "AQAAAAIAAYagAAAAEKf5oZglWRCePUcvOzYL+inrvbtAleDtE41Wz0sXKBNHwWJQgARvX5WnZDdcu5GFew==");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePlaces_PlaceName",
                table: "FavoritePlaces",
                column: "PlaceName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritePlaces");

            //migrationBuilder.UpdateData(
            //    table: "User",
            //    keyColumn: "Id",
            //    keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
            //    column: "PasswordHash",
            //    value: "AQAAAAIAAYagAAAAEMPOM1miqUtGRK0zg+iFsrmy5793GsmatcHs7fXOPzrfGpcQc0PIMPX/YlOvytMl0A==");
        }
    }
}
