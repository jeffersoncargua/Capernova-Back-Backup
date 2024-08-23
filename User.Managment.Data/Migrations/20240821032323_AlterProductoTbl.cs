using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AlterProductoTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07b29794-c0b5-4989-ad17-dcda4abe775f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a4c397f-a0a7-4429-8863-f7647d52720f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6670cee8-cbfc-4f0f-97d4-f34c04d9434a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b556c2d-fa8d-4ac2-9934-66b95125fd0b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca645d23-1cbf-475c-b761-ee45ed3d4e63");

            migrationBuilder.RenameColumn(
                name: "Imagen",
                table: "ProductoTbl",
                newName: "ImagenUrl");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15d7fffd-7399-497a-8ddd-3ae7cf9cf645", "4", "Teacher", "TEACHER" },
                    { "380a255f-8abe-49c3-9530-6992d8d2f78e", "5", "Secretary", "SECRETARY" },
                    { "b22d7c21-0f95-4e9d-91ee-c9285fbe2429", "1", "Admin", "ADMIN" },
                    { "c949d71e-71a4-45e5-9df1-a1edab0e45cc", "3", "Student", "STUDENT" },
                    { "ed50890c-5c9c-41d3-9fd9-204e6e01b973", "2", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15d7fffd-7399-497a-8ddd-3ae7cf9cf645");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "380a255f-8abe-49c3-9530-6992d8d2f78e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b22d7c21-0f95-4e9d-91ee-c9285fbe2429");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c949d71e-71a4-45e5-9df1-a1edab0e45cc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed50890c-5c9c-41d3-9fd9-204e6e01b973");

            migrationBuilder.RenameColumn(
                name: "ImagenUrl",
                table: "ProductoTbl",
                newName: "Imagen");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07b29794-c0b5-4989-ad17-dcda4abe775f", "1", "Admin", "ADMIN" },
                    { "2a4c397f-a0a7-4429-8863-f7647d52720f", "4", "Teacher", "TEACHER" },
                    { "6670cee8-cbfc-4f0f-97d4-f34c04d9434a", "5", "Secretary", "SECRETARY" },
                    { "6b556c2d-fa8d-4ac2-9934-66b95125fd0b", "2", "User", "USER" },
                    { "ca645d23-1cbf-475c-b761-ee45ed3d4e63", "3", "Student", "STUDENT" }
                });
        }
    }
}
