using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Photo = table.Column<string>(type: "varchar(355)", unicode: false, maxLength: 355, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Governor__737584F7DFCB6029", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "IdentityRoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRoleClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserRole", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserToken",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Activities = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Programs__737584F73BE6336D", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Type_of_Tourism",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Photo = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Type_of___737584F7C5CDEEEF", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Country = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Photo = table.Column<string>(type: "varchar(355)", unicode: false, maxLength: 355, nullable: true),
                    Tourguidid = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__3214EC07EBFEDA1A", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Users__Tourguid___1CF15040",
                        column: x => x.Tourguidid,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Photo = table.Column<string>(type: "varchar(355)", unicode: false, maxLength: 355, nullable: true),
                    Location = table.Column<string>(type: "varchar(355)", unicode: false, maxLength: 355, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Government_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Places__737584F77499CA45", x => x.Name);
                    table.ForeignKey(
                        name: "FK__Places__Governme__21B6055D",
                        column: x => x.Government_name,
                        principalTable: "Governorates",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Programs_Photo",
                columns: table => new
                {
                    Program_Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Photo = table.Column<string>(type: "varchar(355)", unicode: false, maxLength: 355, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Programs__983F9C28780D659C", x => new { x.Program_Name, x.Photo });
                    table.ForeignKey(
                        name: "FK__Programs___Progr__3C69FB99",
                        column: x => x.Program_Name,
                        principalTable: "Programs",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Place_Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comments__3214EC07BE7AB12A", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Comments__Place___2B3F6F97",
                        column: x => x.Place_Name,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Program_Places",
                columns: table => new
                {
                    Program_Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Place_Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Program___3088689D29880B4F", x => new { x.Program_Name, x.Place_Name });
                    table.ForeignKey(
                        name: "FK__Program_P__Place__403A8C7D",
                        column: x => x.Place_Name,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Program_P__Progr__3F466844",
                        column: x => x.Program_Name,
                        principalTable: "Programs",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tourguid_Places",
                columns: table => new
                {
                    Tourguidid = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Place_Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tourguid__88E9D762BAF1713A", x => new { x.Tourguidid, x.Place_Name });
                    table.ForeignKey(
                        name: "FK__Tourguid___Place__286302EC",
                        column: x => x.Place_Name,
                        principalTable: "Places",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK__Tourguid___Tourg__276EDEB3",
                        column: x => x.Tourguidid,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Type_of_Tourism_Places",
                columns: table => new
                {
                    Tourism_Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Place_Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Type_of___C27BE45AEE2DFDFA", x => new { x.Tourism_Name, x.Place_Name });
                    table.ForeignKey(
                        name: "FK__Type_of_T__Place__33D4B598",
                        column: x => x.Place_Name,
                        principalTable: "Places",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK__Type_of_T__Touri__32E0915F",
                        column: x => x.Tourism_Name,
                        principalTable: "Type_of_Tourism",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Place_Name",
                table: "Comments",
                column: "Place_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Places_Government_name",
                table: "Places",
                column: "Government_name");

            migrationBuilder.CreateIndex(
                name: "IX_Program_Places_Place_Name",
                table: "Program_Places",
                column: "Place_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tourguid_Places_Place_Name",
                table: "Tourguid_Places",
                column: "Place_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Type_of_Tourism_Places_Place_Name",
                table: "Type_of_Tourism_Places",
                column: "Place_Name");

            migrationBuilder.CreateIndex(
                name: "IX_User_Tourguidid",
                table: "User",
                column: "Tourguidid");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D10534F4661017",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "IdentityRoleClaim");

            migrationBuilder.DropTable(
                name: "IdentityUserClaim");

            migrationBuilder.DropTable(
                name: "IdentityUserLogin");

            migrationBuilder.DropTable(
                name: "IdentityUserRole");

            migrationBuilder.DropTable(
                name: "IdentityUserToken");

            migrationBuilder.DropTable(
                name: "Program_Places");

            migrationBuilder.DropTable(
                name: "Programs_Photo");

            migrationBuilder.DropTable(
                name: "Tourguid_Places");

            migrationBuilder.DropTable(
                name: "Type_of_Tourism_Places");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Type_of_Tourism");

            migrationBuilder.DropTable(
                name: "Governorates");
        }
    }
}
