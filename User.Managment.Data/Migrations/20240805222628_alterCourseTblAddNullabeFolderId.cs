using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class alterCourseTblAddNullabeFolderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22730eb9-78a5-44f3-a16f-e11462218690");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a08ec44-4b7e-4fc8-bd6f-9324c3730459");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a8cea08-3afd-4ee7-9b99-a30108232d05");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ebf49a25-e884-40e9-99c8-a773fbc71d04");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fe9f7ac4-09ac-4803-a12e-b179b270b963");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "40ae5cc6-cd6b-4896-ad26-43791cd11ca6", "1", "Admin", "ADMIN" },
                    { "46787fc5-70f5-4de1-9583-a62547593915", "2", "User", "USER" },
                    { "7570fc81-ea70-42b5-9b05-9bb2730a08bf", "5", "Secretary", "SECRETARY" },
                    { "7ffef63c-4b44-41d2-8c73-7a54908f45f0", "4", "Teacher", "TEACHER" },
                    { "af821abe-9f56-484b-b7ec-251cc5d92c49", "3", "Student", "STUDENT" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40ae5cc6-cd6b-4896-ad26-43791cd11ca6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46787fc5-70f5-4de1-9583-a62547593915");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7570fc81-ea70-42b5-9b05-9bb2730a08bf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ffef63c-4b44-41d2-8c73-7a54908f45f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af821abe-9f56-484b-b7ec-251cc5d92c49");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "22730eb9-78a5-44f3-a16f-e11462218690", "5", "Secretary", "SECRETARY" },
                    { "7a08ec44-4b7e-4fc8-bd6f-9324c3730459", "3", "Student", "STUDENT" },
                    { "7a8cea08-3afd-4ee7-9b99-a30108232d05", "4", "Teacher", "TEACHER" },
                    { "ebf49a25-e884-40e9-99c8-a773fbc71d04", "1", "Admin", "ADMIN" },
                    { "fe9f7ac4-09ac-4803-a12e-b179b270b963", "2", "User", "USER" }
                });
        }
    }
}
