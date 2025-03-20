using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class updateplaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaceUser");

            migrationBuilder.AddColumn<string>(
                name: "VisitingHours",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.UpdateData(
            //    table: "User",
            //    keyColumn: "Id",
            //    keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
            //    column: "PasswordHash",
            //    value: "AQAAAAIAAYagAAAAEMPOM1miqUtGRK0zg+iFsrmy5793GsmatcHs7fXOPzrfGpcQc0PIMPX/YlOvytMl0A==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitingHours",
                table: "Places");

            migrationBuilder.CreateTable(
                name: "PlaceUser",
                columns: table => new
                {
                    PlaceNamesName = table.Column<string>(type: "varchar(255)", nullable: false),
                    TourguidsId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceUser", x => new { x.PlaceNamesName, x.TourguidsId });
                    table.ForeignKey(
                        name: "FK_PlaceUser_Places_PlaceNamesName",
                        column: x => x.PlaceNamesName,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaceUser_User_TourguidsId",
                        column: x => x.TourguidsId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKgW52AnYcZvjr8Rmf5JSXdHPhUkeNVQh6W/yeVI0sikTAszQJwyP7XZ4Zh50sQpsA==");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceUser_TourguidsId",
                table: "PlaceUser",
                column: "TourguidsId");
        }
    }
}
