using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AlterPedidoTblAddEstado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a52cc55-2ec0-4c70-ab47-0ae92375239d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e066ab6-e8f7-4ba4-8981-9b506c2989b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a25dbf13-9f7a-4841-baa9-664e0120a1f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2355967-ed2b-439f-9218-e3f97dada559");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0a1e1a8-12ae-47d9-b818-50a7f24162a6");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "PedidoTbl",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "PedidoTbl");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a52cc55-2ec0-4c70-ab47-0ae92375239d", "1", "Admin", "ADMIN" },
                    { "7e066ab6-e8f7-4ba4-8981-9b506c2989b3", "2", "User", "USER" },
                    { "a25dbf13-9f7a-4841-baa9-664e0120a1f0", "5", "Secretary", "SECRETARY" },
                    { "e2355967-ed2b-439f-9218-e3f97dada559", "3", "Student", "STUDENT" },
                    { "f0a1e1a8-12ae-47d9-b818-50a7f24162a6", "4", "Teacher", "TEACHER" }
                });
        }
    }
}
