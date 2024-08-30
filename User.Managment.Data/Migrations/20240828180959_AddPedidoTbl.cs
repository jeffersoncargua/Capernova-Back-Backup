using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AddPedidoTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a346fb8-12b2-4c23-aa11-0d41673e0837");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f45430d-cd35-486a-a23b-2e17ca27967c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66dc0cb7-827d-4409-8002-fab756f843fd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "809f5a24-8575-492b-894e-446f6872ce6e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0801c74-76f8-4587-aa36-5a644f529670");

            migrationBuilder.AddColumn<string>(
                name: "ProductoImagen",
                table: "ShoppingCartTbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PedidoTbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Productos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VentaId = table.Column<int>(type: "int", nullable: false),
                    DirectionMain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectionSec = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoTbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoTbl_VentaTbl_VentaId",
                        column: x => x.VentaId,
                        principalTable: "VentaTbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PedidoTbl_VentaId",
                table: "PedidoTbl",
                column: "VentaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoTbl");

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

            migrationBuilder.DropColumn(
                name: "ProductoImagen",
                table: "ShoppingCartTbl");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2a346fb8-12b2-4c23-aa11-0d41673e0837", "4", "Teacher", "TEACHER" },
                    { "3f45430d-cd35-486a-a23b-2e17ca27967c", "2", "User", "USER" },
                    { "66dc0cb7-827d-4409-8002-fab756f843fd", "1", "Admin", "ADMIN" },
                    { "809f5a24-8575-492b-894e-446f6872ce6e", "5", "Secretary", "SECRETARY" },
                    { "b0801c74-76f8-4587-aa36-5a644f529670", "3", "Student", "STUDENT" }
                });
        }
    }
}
