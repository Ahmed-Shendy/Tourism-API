using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTourguidRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tourguid_Rates",
                columns: table => new
                {
                    tourguidId = table.Column<string>(type: "varchar(255)", nullable: false),
                    userId = table.Column<string>(type: "varchar(255)", nullable: false),
                    rate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tourguid_Rates", x => new { x.tourguidId, x.userId });
                    table.CheckConstraint("CK_YourEntity_Rate_Range", "[rate] >= 1 AND [rate] <= 5");
                    table.ForeignKey(
                        name: "FK_Tourguid_Rates_User_tourguidId",
                        column: x => x.tourguidId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tourguid_Rates_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHruxA8bdw4q8VwozrUeU6JiVeWwkGTiG4PZC85ODangUbfWLMJF2d8u+DHMTL7siw==");

            migrationBuilder.CreateIndex(
                name: "IX_Tourguid_Rates_userId",
                table: "Tourguid_Rates",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tourguid_Rates");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO09n5J/1ATzRUY35NwRGSZNyQDG0GO1+j5DRkROx3gGQxOTgntXPkc9dqGJET7PKw==");
        }
    }
}
