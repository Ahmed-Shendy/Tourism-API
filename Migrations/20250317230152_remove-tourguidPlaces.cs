using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class removetourguidPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TourguidPlace");

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

            //migrationBuilder.UpdateData(
            //    table: "User",
            //    keyColumn: "Id",
            //    keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
            //    column: "PasswordHash",
            //    value: "AQAAAAIAAYagAAAAEKgW52AnYcZvjr8Rmf5JSXdHPhUkeNVQh6W/yeVI0sikTAszQJwyP7XZ4Zh50sQpsA==");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PlaceUser_TourguidsId",
            //    table: "PlaceUser",
            //    column: "TourguidsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaceUser");

            migrationBuilder.CreateTable(
                name: "TourguidPlace",
                columns: table => new
                {
                    PlaceName = table.Column<string>(type: "varchar(255)", nullable: false),
                    TourguidId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourguidPlace", x => new { x.PlaceName, x.TourguidId });
                    table.ForeignKey(
                        name: "FK__Tourguid___Place__286302EC",
                        column: x => x.PlaceName,
                        principalTable: "Places",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK__Tourguid___Tourg__276EDEB3",
                        column: x => x.TourguidId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE8sK+bQTuyuPpxpYiQh1BW6GAwwMVMZlEpo79u9v8fZFltL0mESZoWSKokzUVrOVw==");

            migrationBuilder.CreateIndex(
                name: "IX_TourguidPlace_TourguidId",
                table: "TourguidPlace",
                column: "TourguidId");
        }
    }
}
