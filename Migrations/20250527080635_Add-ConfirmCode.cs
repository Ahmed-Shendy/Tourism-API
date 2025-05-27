using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmCode",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "ConfirmCode", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEEn+zArNtWuudj6SaKKcGofKVAdbFaycI9zqQ29qTXgLxP9YmDinqblRbP1KPXdujg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmCode",
                table: "User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJY/Ce/I+Cgg58V6e5RDIPwpvEASodTE1AAdp8rYTs6+egwYCfQ6pRDbGeOK3g29BQ==");
        }
    }
}
