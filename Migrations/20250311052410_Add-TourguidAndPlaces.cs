using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTourguidAndPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TourguidAndPlaces",
                columns: table => new
                {
                    TouguidId = table.Column<string>(type: "varchar(255)", nullable: false),
                    PlaceName = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourguidAndPlaces", x => new { x.TouguidId, x.PlaceName });
                    table.ForeignKey(
                        name: "FK_TourguidAndPlaces_Places_PlaceName",
                        column: x => x.PlaceName,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourguidAndPlaces_User_TouguidId",
                        column: x => x.TouguidId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TourguidAndPlaces_PlaceName",
                table: "TourguidAndPlaces",
                column: "PlaceName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TourguidAndPlaces");
        }
    }
}
