using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AddMatriculacionAndShoppingCartAndProductoAndFacturacionTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "219ad934-0d25-4021-9392-a6f155d94a38");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ac1e30f-e6bf-44bd-a4c3-5d22d0e26b9d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65189451-c8c2-46bc-8b1d-5aba0490223e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b2e14c0-f5f2-44c9-bde5-953cbbc9dbbb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8a78efa-cc5b-4a62-8d21-bcfbc63367c4");

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

            migrationBuilder.CreateTable(
                name: "MatriculaTbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CursoId = table.Column<int>(type: "int", nullable: false),
                    EstudianteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotaFinal = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatriculaTbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatriculaTbl_CourseTbl_CursoId",
                        column: x => x.CursoId,
                        principalTable: "CourseTbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatriculaTbl_StudentTbl_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "StudentTbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductoTbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detalle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoTbl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartTbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartTbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartTbl_FacturacionTbl_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "FacturacionTbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartTbl_ProductoTbl_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "ProductoTbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "48190452-554a-44e5-9673-6050d9205e58", "1", "Admin", "ADMIN" },
                    { "7e0812a6-feee-4585-8b64-9fe181d67d45", "5", "Secretary", "SECRETARY" },
                    { "9f68fefa-94ab-4bd1-9770-736d1eca66f4", "4", "Teacher", "TEACHER" },
                    { "b3b33d82-ffdc-4ec8-af14-b5aa79406fef", "2", "User", "USER" },
                    { "cbb2734f-d32b-4b77-9b22-fab36e758b18", "3", "Student", "STUDENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacturacionTbl_UserId",
                table: "FacturacionTbl",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MatriculaTbl_CursoId",
                table: "MatriculaTbl",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_MatriculaTbl_EstudianteId",
                table: "MatriculaTbl",
                column: "EstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoTbl_Codigo",
                table: "ProductoTbl",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartTbl_FacturaId",
                table: "ShoppingCartTbl",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartTbl_ProductoId",
                table: "ShoppingCartTbl",
                column: "ProductoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatriculaTbl");

            migrationBuilder.DropTable(
                name: "ShoppingCartTbl");

            migrationBuilder.DropTable(
                name: "FacturacionTbl");

            migrationBuilder.DropTable(
                name: "ProductoTbl");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48190452-554a-44e5-9673-6050d9205e58");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e0812a6-feee-4585-8b64-9fe181d67d45");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f68fefa-94ab-4bd1-9770-736d1eca66f4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3b33d82-ffdc-4ec8-af14-b5aa79406fef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cbb2734f-d32b-4b77-9b22-fab36e758b18");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "219ad934-0d25-4021-9392-a6f155d94a38", "3", "Student", "STUDENT" },
                    { "2ac1e30f-e6bf-44bd-a4c3-5d22d0e26b9d", "5", "Secretary", "SECRETARY" },
                    { "65189451-c8c2-46bc-8b1d-5aba0490223e", "4", "Teacher", "TEACHER" },
                    { "7b2e14c0-f5f2-44c9-bde5-953cbbc9dbbb", "1", "Admin", "ADMIN" },
                    { "f8a78efa-cc5b-4a62-8d21-bcfbc63367c4", "2", "User", "USER" }
                });
        }
    }
}
