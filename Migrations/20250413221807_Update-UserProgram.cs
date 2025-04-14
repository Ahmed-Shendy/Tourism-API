using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAswers");

            migrationBuilder.CreateTable(
                name: "UserProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProgramName = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProgram_Programs_ProgramName",
                        column: x => x.ProgramName,
                        principalTable: "Programs",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProgram_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJQYDWaXxbP9HkvbiKLMPA2fAlwXS98lh9lahX3lLNfwDPQ9cW3cNASdtq6YzfkHpQ==");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgram_ProgramName",
                table: "UserProgram",
                column: "ProgramName");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgram_UserId",
                table: "UserProgram",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProgram");

            migrationBuilder.CreateTable(
                name: "UserAswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramName = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Question1 = table.Column<bool>(type: "bit", nullable: false),
                    Question10 = table.Column<bool>(type: "bit", nullable: false),
                    Question2 = table.Column<bool>(type: "bit", nullable: false),
                    Question3 = table.Column<bool>(type: "bit", nullable: false),
                    Question4 = table.Column<bool>(type: "bit", nullable: false),
                    Question5 = table.Column<bool>(type: "bit", nullable: false),
                    Question6 = table.Column<bool>(type: "bit", nullable: false),
                    Question7 = table.Column<bool>(type: "bit", nullable: false),
                    Question8 = table.Column<bool>(type: "bit", nullable: false),
                    Question9 = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAswers_Programs_ProgramName",
                        column: x => x.ProgramName,
                        principalTable: "Programs",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAswers_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH0dsmKOX3d7O4RxqzkRmD2lB8xAxLoexT7iudChry3phTNoGOb4bToBjK4ersBagQ==");

            migrationBuilder.CreateIndex(
                name: "IX_UserAswers_ProgramName",
                table: "UserAswers",
                column: "ProgramName");

            migrationBuilder.CreateIndex(
                name: "IX_UserAswers_UserId",
                table: "UserAswers",
                column: "UserId");
        }
    }
}
