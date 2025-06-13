using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMoveToPlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoveToPlace",
                table: "TourguidAndPlaces");

            migrationBuilder.RenameColumn(
                name: "MoveToTrip",
                table: "User",
                newName: "MoveTo");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBMnCRbP69pGci5lH9H243BoQyZM/EIqXylHUgIwAhhZHtLkZbTYuSrkLzlZEX4vmQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoveTo",
                table: "User",
                newName: "MoveToTrip");

            migrationBuilder.AddColumn<string>(
                name: "MoveToPlace",
                table: "TourguidAndPlaces",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL9b9LpfzE741UmxQzNJteZoSt/cA4aV944uHx5B5msVDSjKcBdHD5bYxEFyPv7zJg==");
        }
    }
}
