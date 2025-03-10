using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRatePlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "placeRates",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    PlaceName = table.Column<string>(type: "varchar(255)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_placeRates", x => new { x.UserId, x.PlaceName });
                    table.ForeignKey(
                        name: "FK_placeRates_Places_PlaceName",
                        column: x => x.PlaceName,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_placeRates_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_placeRates_PlaceName",
                table: "placeRates",
                column: "PlaceName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "placeRates");
        }
    }
}
