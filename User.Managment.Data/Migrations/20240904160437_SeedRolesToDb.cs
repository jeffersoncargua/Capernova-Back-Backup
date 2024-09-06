using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class SeedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "69d7d589-ebb5-44f7-a883-895123edf64e", "4", "Teacher", "TEACHER" },
                    { "7b4b23fd-c3bd-410d-8097-5c953efb5906", "3", "Student", "STUDENT" },
                    { "84f27c24-df7b-4926-96b1-d43ee4ed4283", "2", "User", "USER" },
                    { "aac42112-eaef-426c-acb7-e45a5254cec4", "1", "Admin", "ADMIN" },
                    { "e034127d-5089-466d-a4bf-572371daef8d", "5", "Secretary", "SECRETARY" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69d7d589-ebb5-44f7-a883-895123edf64e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b4b23fd-c3bd-410d-8097-5c953efb5906");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84f27c24-df7b-4926-96b1-d43ee4ed4283");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aac42112-eaef-426c-acb7-e45a5254cec4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e034127d-5089-466d-a4bf-572371daef8d");
        }
    }
}
