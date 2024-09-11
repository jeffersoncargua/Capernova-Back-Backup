using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AddRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0c2a93d5-b4c2-4c33-9f74-0d7d906090e5", "3", "Student", "STUDENT" },
                    { "27bdeae2-c5bc-4221-b86c-a6736fa3a296", "2", "User", "USER" },
                    { "499cf5f9-26bf-4b96-bced-2f7ea855e53d", "5", "Secretary", "SECRETARY" },
                    { "8d20a49c-bfdd-4327-92ee-c74b348b26f0", "4", "Teacher", "TEACHER" },
                    { "db5971d6-9d26-4162-b539-43f64b43f5b9", "1", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c2a93d5-b4c2-4c33-9f74-0d7d906090e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27bdeae2-c5bc-4221-b86c-a6736fa3a296");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "499cf5f9-26bf-4b96-bced-2f7ea855e53d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d20a49c-bfdd-4327-92ee-c74b348b26f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db5971d6-9d26-4162-b539-43f64b43f5b9");
        }
    }
}
