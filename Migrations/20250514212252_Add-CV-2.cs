using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvContentType",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "CvContentType", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEA6MfVnYZiP4nH9NOLOMrXxNye3mgCvGAMKlSiKGAxyzuC4lQgOeOa4uX9sSXt3/qA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvContentType",
                table: "User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEvsdPjNXaGb7Z8yHJD7KHt5KtWnrcpLqnfmkURgCaOxIVejGUrgCTj9iA3UQRG3Hg==");
        }
    }
}
