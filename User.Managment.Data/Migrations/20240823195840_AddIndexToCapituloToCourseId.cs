using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AddIndexToCapituloToCourseId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4963f4ca-3b0e-46ac-be34-606e2544d265");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85093123-61ee-4ba3-b4c1-da9cc7ef9d86");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d280250d-77c0-4efe-977e-364f14de445f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9468390-25a1-4115-a182-6e4fc789289a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb5f07f1-06d3-45d2-ab96-6cbcb0da117e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0623fedb-e3fb-4da8-a15a-ba095f2f3ccb", "4", "Teacher", "TEACHER" },
                    { "114bdc25-daec-4d01-9a62-dde4722f2799", "3", "Student", "STUDENT" },
                    { "282e6064-5928-4a9e-a18d-58059cc13a91", "2", "User", "USER" },
                    { "4061c24d-04e4-427b-9e4a-56fe195c28ea", "5", "Secretary", "SECRETARY" },
                    { "5ce3e3e4-d455-4d79-a051-2d131c92c182", "1", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0623fedb-e3fb-4da8-a15a-ba095f2f3ccb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "114bdc25-daec-4d01-9a62-dde4722f2799");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "282e6064-5928-4a9e-a18d-58059cc13a91");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4061c24d-04e4-427b-9e4a-56fe195c28ea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ce3e3e4-d455-4d79-a051-2d131c92c182");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4963f4ca-3b0e-46ac-be34-606e2544d265", "5", "Secretary", "SECRETARY" },
                    { "85093123-61ee-4ba3-b4c1-da9cc7ef9d86", "2", "User", "USER" },
                    { "d280250d-77c0-4efe-977e-364f14de445f", "3", "Student", "STUDENT" },
                    { "e9468390-25a1-4115-a182-6e4fc789289a", "4", "Teacher", "TEACHER" },
                    { "fb5f07f1-06d3-45d2-ab96-6cbcb0da117e", "1", "Admin", "ADMIN" }
                });
        }
    }
}
