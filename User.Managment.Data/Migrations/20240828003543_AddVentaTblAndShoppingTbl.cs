using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AddVentaTblAndShoppingTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartTbl_FacturacionTbl_FacturaId",
                table: "ShoppingCartTbl");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartTbl_ProductoTbl_ProductoId",
                table: "ShoppingCartTbl");

            migrationBuilder.DropTable(
                name: "FacturacionTbl");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartTbl_FacturaId",
                table: "ShoppingCartTbl");

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

            migrationBuilder.DropColumn(
                name: "FacturaId",
                table: "ShoppingCartTbl");

            migrationBuilder.RenameColumn(
                name: "ProductoId",
                table: "ShoppingCartTbl",
                newName: "VentaId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartTbl_ProductoId",
                table: "ShoppingCartTbl",
                newName: "IX_ShoppingCartTbl_VentaId");

            migrationBuilder.AlterColumn<int>(
                name: "Cantidad",
                table: "ShoppingCartTbl",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "ProductoCode",
                table: "ShoppingCartTbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductoName",
                table: "ShoppingCartTbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "ShoppingCartTbl",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectionMain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectionSec = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VentaTbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClienteId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaTbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentaTbl_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VentaTbl_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7eca842e-8ef3-4e6e-9147-a57497a521e5", "3", "Student", "STUDENT" },
                    { "a6a03fc1-d43c-4781-aeef-d77687a44904", "4", "Teacher", "TEACHER" },
                    { "d90745f7-5c5a-4f9a-baad-833d53802583", "1", "Admin", "ADMIN" },
                    { "f85fc566-f4ea-43b4-a4ca-e8405a01bc8d", "2", "User", "USER" },
                    { "fe64a82b-8583-42b1-9c24-260f852b60e3", "5", "Secretary", "SECRETARY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VentaTbl_ClienteId",
                table: "VentaTbl",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_VentaTbl_Emision",
                table: "VentaTbl",
                column: "Emision");

            migrationBuilder.CreateIndex(
                name: "IX_VentaTbl_UserId",
                table: "VentaTbl",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartTbl_VentaTbl_VentaId",
                table: "ShoppingCartTbl",
                column: "VentaId",
                principalTable: "VentaTbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartTbl_VentaTbl_VentaId",
                table: "ShoppingCartTbl");

            migrationBuilder.DropTable(
                name: "VentaTbl");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7eca842e-8ef3-4e6e-9147-a57497a521e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6a03fc1-d43c-4781-aeef-d77687a44904");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d90745f7-5c5a-4f9a-baad-833d53802583");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f85fc566-f4ea-43b4-a4ca-e8405a01bc8d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fe64a82b-8583-42b1-9c24-260f852b60e3");

            migrationBuilder.DropColumn(
                name: "ProductoCode",
                table: "ShoppingCartTbl");

            migrationBuilder.DropColumn(
                name: "ProductoName",
                table: "ShoppingCartTbl");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "ShoppingCartTbl");

            migrationBuilder.RenameColumn(
                name: "VentaId",
                table: "ShoppingCartTbl",
                newName: "ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartTbl_VentaId",
                table: "ShoppingCartTbl",
                newName: "IX_ShoppingCartTbl_ProductoId");

            migrationBuilder.AlterColumn<double>(
                name: "Cantidad",
                table: "ShoppingCartTbl",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FacturaId",
                table: "ShoppingCartTbl",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FacturacionTbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaVenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturacionTbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacturacionTbl_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartTbl_FacturaId",
                table: "ShoppingCartTbl",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_FacturacionTbl_UserId",
                table: "FacturacionTbl",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartTbl_FacturacionTbl_FacturaId",
                table: "ShoppingCartTbl",
                column: "FacturaId",
                principalTable: "FacturacionTbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartTbl_ProductoTbl_ProductoId",
                table: "ShoppingCartTbl",
                column: "ProductoId",
                principalTable: "ProductoTbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
