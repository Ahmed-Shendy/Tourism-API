using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class addTrips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TripName",
                table: "User",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Days = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    programName = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Trips_Programs_programName",
                        column: x => x.programName,
                        principalTable: "Programs",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripsPlaces",
                columns: table => new
                {
                    TripName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlaceName = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripsPlaces", x => new { x.TripName, x.PlaceName });
                    table.ForeignKey(
                        name: "FK_TripsPlaces_Places_PlaceName",
                        column: x => x.PlaceName,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripsPlaces_Trips_TripName",
                        column: x => x.TripName,
                        principalTable: "Trips",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "PasswordHash", "TripName" },
                values: new object[] { "AQAAAAIAAYagAAAAEH0dsmKOX3d7O4RxqzkRmD2lB8xAxLoexT7iudChry3phTNoGOb4bToBjK4ersBagQ==", null });

            migrationBuilder.CreateIndex(
                name: "IX_User_TripName",
                table: "User",
                column: "TripName");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_programName",
                table: "Trips",
                column: "programName");

            migrationBuilder.CreateIndex(
                name: "IX_TripsPlaces_PlaceName",
                table: "TripsPlaces",
                column: "PlaceName");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Trips_TripName",
                table: "User",
                column: "TripName",
                principalTable: "Trips",
                principalColumn: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Trips_TripName",
                table: "User");

            migrationBuilder.DropTable(
                name: "TripsPlaces");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_User_TripName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TripName",
                table: "User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAx5rN4FeD3WkW97SZ44/67Xkzy/XE29cq4li7TyLWNCxJtlsanAbSFd669atGpz8w==");
        }
    }
}
