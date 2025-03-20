using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tourism_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__Tourguid__88E9D762BAF1713A",
                table: "Tourguid_Places");

            migrationBuilder.DropIndex(
                name: "IX_Tourguid_Places_Place_Name",
                table: "Tourguid_Places");

            migrationBuilder.RenameTable(
                name: "Tourguid_Places",
                newName: "TourguidPlace");

            migrationBuilder.RenameColumn(
                name: "Tourguidid",
                table: "TourguidPlace",
                newName: "TourguidId");

            migrationBuilder.RenameColumn(
                name: "Place_Name",
                table: "TourguidPlace",
                newName: "PlaceName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourguidPlace",
                table: "TourguidPlace",
                columns: new[] { "PlaceName", "TourguidId" });

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
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "Country", "Email", "EmailConfirmed", "Gender", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "Phone", "PhoneNumber", "PhoneNumberConfirmed", "Photo", "Role", "SecurityStamp", "Tourguidid", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6dc6528a-b280-4770-9eae-82671ee81ef7", 0, null, "99d2bbc6-bc54-4248-a172-a77de3ae4430", "Egypt", "admin@gmail.com", true, "Male", false, null, "Admin", "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "P@ssword123", "AQAAAAIAAYagAAAAEE8sK+bQTuyuPpxpYiQh1BW6GAwwMVMZlEpo79u9v8fZFltL0mESZoWSKokzUVrOVw==", "01151813561", null, false, null, "Admin", "55BF92C9EF0249CDA210D85D1A851BC9", null, false, "admin@gmail.com" });

            migrationBuilder.CreateIndex(
                name: "IX_TourguidPlace_TourguidId",
                table: "TourguidPlace",
                column: "TourguidId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TourguidPlace",
                table: "TourguidPlace");

            migrationBuilder.DropIndex(
                name: "IX_TourguidPlace_TourguidId",
                table: "TourguidPlace");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92b75286-d8f8-4061-9995-e6e23ccdee94");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f78b99da-c771-4b4a-8c85-e83079dc228c");

            migrationBuilder.DeleteData(
                table: "IdentityUserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "92b75286-d8f8-4061-9995-e6e23ccdee94", "6dc6528a-b280-4770-9eae-82671ee81ef7" });

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7");

            migrationBuilder.RenameTable(
                name: "TourguidPlace",
                newName: "Tourguid_Places");

            migrationBuilder.RenameColumn(
                name: "TourguidId",
                table: "Tourguid_Places",
                newName: "Tourguidid");

            migrationBuilder.RenameColumn(
                name: "PlaceName",
                table: "Tourguid_Places",
                newName: "Place_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Tourguid__88E9D762BAF1713A",
                table: "Tourguid_Places",
                columns: new[] { "Tourguidid", "Place_Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Tourguid_Places_Place_Name",
                table: "Tourguid_Places",
                column: "Place_Name");
        }
    }
}
