using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AddSeedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5e46982b-d323-4dba-8d0d-ee7df86017d8", "1", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b68dc162-b1df-4518-895b-95cdab11612d", "1", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bbaafa37-c699-4c86-9af3-0277ddd630ae", "1", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e46982b-d323-4dba-8d0d-ee7df86017d8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b68dc162-b1df-4518-895b-95cdab11612d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbaafa37-c699-4c86-9af3-0277ddd630ae");
        }
    }
}
