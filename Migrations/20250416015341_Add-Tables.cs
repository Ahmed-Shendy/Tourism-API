using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                    Photo = table.Column<string>(type: "text", unicode: false, maxLength: 355, nullable: true)
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
                    Photo = table.Column<string>(type: "text", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Type_of___737584F7C5CDEEEF", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Photo = table.Column<string>(type: "text", unicode: false, maxLength: 355, nullable: true),
                    Location = table.Column<string>(type: "text", unicode: false, maxLength: 355, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0.0m),
                    VisitingHours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Government_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    GoogleRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0.0m)
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
                name: "Type_of_Tourism_Places",
                columns: table => new
                {
                    Tourism_Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Place_Name = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_of_Tourism_Places", x => new { x.Tourism_Name, x.Place_Name });
                    table.ForeignKey(
                        name: "FK_Type_of_Tourism_Places_Places_Place_Name",
                        column: x => x.Place_Name,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Type_of_Tourism_Places_Type_of_Tourism_Tourism_Name",
                        column: x => x.Tourism_Name,
                        principalTable: "Type_of_Tourism",
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
                    Age = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    Role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Photo = table.Column<string>(type: "text", unicode: false, maxLength: 355, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tourguidid = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ProgramName = table.Column<string>(type: "varchar(255)", nullable: true),
                    TripName = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        name: "FK_User_Programs_ProgramName",
                        column: x => x.ProgramName,
                        principalTable: "Programs",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_User_Trips_TripName",
                        column: x => x.TripName,
                        principalTable: "Trips",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK__Users__Tourguid___1CF15040",
                        column: x => x.Tourguidid,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "text", unicode: false, nullable: false),
                    Place_Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
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
                    table.ForeignKey(
                        name: "FK__Comments__UserI__2A4B4B5E",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoritePlaces",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    PlaceName = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritePlaces", x => new { x.UserId, x.PlaceName });
                    table.ForeignKey(
                        name: "FK_FavoritePlaces_Places_PlaceName",
                        column: x => x.PlaceName,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoritePlaces_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "placeRates",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    PlaceName = table.Column<string>(type: "varchar(255)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_placeRates", x => new { x.UserId, x.PlaceName });
                    table.ForeignKey(
                        name: "FK_placeRates_Places_PlaceName",
                        column: x => x.PlaceName,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_placeRates_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => new { x.UserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourguidAndPlaces",
                columns: table => new
                {
                    TouguidId = table.Column<string>(type: "varchar(255)", nullable: false),
                    PlaceName = table.Column<string>(type: "varchar(255)", nullable: false),
                    MoveToPlace = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourguidAndPlaces", x => x.TouguidId);
                    table.ForeignKey(
                        name: "FK_TourguidAndPlaces_Places_PlaceName",
                        column: x => x.PlaceName,
                        principalTable: "Places",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourguidAndPlaces_User_TouguidId",
                        column: x => x.TouguidId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92b75286-d8f8-4061-9995-e6e23ccdee94", "f51e5a91-bced-49c2-8b86-c2e170c0846c", false, false, "Admin", "ADMIN" },
                    { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "5ee6bc12-5cb0-4304-91e7-6a00744e042a", true, false, "Member", "MEMBER" },
                    { "f78b99da-c771-4b4a-8c85-e83079dc228c", "4f98002a-a963-4043-a4e5-100592050714", false, false, "Tourguid", "TOURGUID" }
                });

            migrationBuilder.InsertData(
                table: "IdentityUserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "92b75286-d8f8-4061-9995-e6e23ccdee94", "6dc6528a-b280-4770-9eae-82671ee81ef7" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "Age", "ConcurrencyStamp", "ContentType", "Country", "Email", "EmailConfirmed", "Gender", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "Phone", "PhoneNumber", "PhoneNumberConfirmed", "Photo", "ProgramName", "Role", "Score", "SecurityStamp", "Tourguidid", "TripName", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6dc6528a-b280-4770-9eae-82671ee81ef7", 0, null, "99d2bbc6-bc54-4248-a172-a77de3ae4430", null, "Egypt", "admin@gmail.com", true, "Male", false, null, "Admin", "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "P@ssword123", "AQAAAAIAAYagAAAAEO09n5J/1ATzRUY35NwRGSZNyQDG0GO1+j5DRkROx3gGQxOTgntXPkc9dqGJET7PKw==", "01151813561", null, false, null, null, "Admin", 0m, "55BF92C9EF0249CDA210D85D1A851BC9", null, null, false, "admin@gmail.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Place_Name",
                table: "Comments",
                column: "Place_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePlaces_PlaceName",
                table: "FavoritePlaces",
                column: "PlaceName");

            migrationBuilder.CreateIndex(
                name: "IX_placeRates_PlaceName",
                table: "placeRates",
                column: "PlaceName");

            migrationBuilder.CreateIndex(
                name: "IX_Places_Government_name",
                table: "Places",
                column: "Government_name");

            migrationBuilder.CreateIndex(
                name: "IX_Program_Places_Place_Name",
                table: "Program_Places",
                column: "Place_Name");

            migrationBuilder.CreateIndex(
                name: "IX_TourguidAndPlaces_PlaceName",
                table: "TourguidAndPlaces",
                column: "PlaceName");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_programName",
                table: "Trips",
                column: "programName");

            migrationBuilder.CreateIndex(
                name: "IX_TripsPlaces_PlaceName",
                table: "TripsPlaces",
                column: "PlaceName");

            migrationBuilder.CreateIndex(
                name: "IX_Type_of_Tourism_Places_Place_Name",
                table: "Type_of_Tourism_Places",
                column: "Place_Name");

            migrationBuilder.CreateIndex(
                name: "IX_User_ProgramName",
                table: "User",
                column: "ProgramName");

            migrationBuilder.CreateIndex(
                name: "IX_User_Tourguidid",
                table: "User",
                column: "Tourguidid");

            migrationBuilder.CreateIndex(
                name: "IX_User_TripName",
                table: "User",
                column: "TripName");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D10534F4661017",
                table: "User",
                column: "Email",
                unique: true);

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
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "FavoritePlaces");

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
                name: "placeRates");

            migrationBuilder.DropTable(
                name: "Program_Places");

            migrationBuilder.DropTable(
                name: "Programs_Photo");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "TourguidAndPlaces");

            migrationBuilder.DropTable(
                name: "TripsPlaces");

            migrationBuilder.DropTable(
                name: "Type_of_Tourism_Places");

            migrationBuilder.DropTable(
                name: "UserProgram");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Type_of_Tourism");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Programs");
        }
    }
}
