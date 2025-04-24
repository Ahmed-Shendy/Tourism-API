using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class Updatetourguid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentTouristsCount",
                table: "User",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxTourists",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CurrentTouristsCount", "IsActive", "MaxTourists", "PasswordHash" },
                values: new object[] { 0, true, null, "AQAAAAIAAYagAAAAEJZdwmMX94a7DA0TeRgWFF+7ee9XG/B8cwq/iT2jbEw0SSmelCw57BIh/JNjm9ja4A==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentTouristsCount",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "User");

            migrationBuilder.DropColumn(
                name: "MaxTourists",
                table: "User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBq83w75vmWemVhsoOuOrWcCXALtbq+dz+3pH60itwFVRqWoZvucb3o7ialy7Jw/bQ==");
        }
    }
}
