using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question1 = table.Column<bool>(type: "bit", nullable: false),
                    Question2 = table.Column<bool>(type: "bit", nullable: false),
                    Question3 = table.Column<bool>(type: "bit", nullable: false),
                    Question4 = table.Column<bool>(type: "bit", nullable: false),
                    Question5 = table.Column<bool>(type: "bit", nullable: false),
                    Question6 = table.Column<bool>(type: "bit", nullable: false),
                    Question7 = table.Column<bool>(type: "bit", nullable: false),
                    Question8 = table.Column<bool>(type: "bit", nullable: false),
                    Question9 = table.Column<bool>(type: "bit", nullable: false),
                    Question10 = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProgramName = table.Column<string>(type: "varchar(255)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_UserAswers_ProgramName",
                table: "UserAswers",
                column: "ProgramName");

            migrationBuilder.CreateIndex(
                name: "IX_UserAswers_UserId",
                table: "UserAswers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAswers");
        }
    }
}
