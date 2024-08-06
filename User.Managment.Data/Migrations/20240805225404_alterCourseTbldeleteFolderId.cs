using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class alterCourseTbldeleteFolderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "CourseTbl");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38440e89-2e81-4c48-9eb2-a32356eb377a", "5", "Secretary", "SECRETARY" },
                    { "48986b37-36a5-46ac-a7ac-32c895e02f1d", "2", "User", "USER" },
                    { "60514743-ab06-44e2-8bf5-5544b00885a9", "3", "Student", "STUDENT" },
                    { "9da60f83-cc66-4222-a293-1278327cd86c", "1", "Admin", "ADMIN" },
                    { "cb79cc4a-9dd0-4bbc-8767-b42225091548", "4", "Teacher", "TEACHER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38440e89-2e81-4c48-9eb2-a32356eb377a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48986b37-36a5-46ac-a7ac-32c895e02f1d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60514743-ab06-44e2-8bf5-5544b00885a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9da60f83-cc66-4222-a293-1278327cd86c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb79cc4a-9dd0-4bbc-8767-b42225091548");

            migrationBuilder.AddColumn<string>(
                name: "FolderId",
                table: "CourseTbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
