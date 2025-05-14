using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class Addlangue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Langues",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Langue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourguidId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Langues", x => x.id);
                    table.ForeignKey(
                        name: "FK_Langues_User_TourguidId",
                        column: x => x.TourguidId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENHyVqTt8mtP65qai+9ojqUxm3r8+nWC49+1zAns4aTckIXFmesrDDwmJjraVh5y5A==");

            migrationBuilder.CreateIndex(
                name: "IX_Langues_TourguidId",
                table: "Langues",
                column: "TourguidId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Langues");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKCGyk7xsFCqvoYNBFgBX7LPGpRONZs/rwWswsmoID4cmQFRO6nYjWv4PKylo7bGYg==");
        }
    }
}
