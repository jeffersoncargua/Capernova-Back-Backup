using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class DeleteAreaAndAlterCategoriaTblAndAlterProductoTblAddCategoriaId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaTbl");

            migrationBuilder.DropIndex(
                name: "IX_ProductoTbl_Especificacion",
                table: "ProductoTbl");


            migrationBuilder.DropColumn(
                name: "Especificacion",
                table: "ProductoTbl");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "ProductoTbl",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "CategoriaTbl",
                type: "nvarchar(max)",
                nullable: true);


            migrationBuilder.CreateIndex(
                name: "IX_ProductoTbl_CategoriaId",
                table: "ProductoTbl",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoTbl_CategoriaTbl_CategoriaId",
                table: "ProductoTbl",
                column: "CategoriaId",
                principalTable: "CategoriaTbl",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductoTbl_CategoriaTbl_CategoriaId",
                table: "ProductoTbl");

            migrationBuilder.DropIndex(
                name: "IX_ProductoTbl_CategoriaId",
                table: "ProductoTbl");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "ProductoTbl");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "CategoriaTbl");

            migrationBuilder.AddColumn<string>(
                name: "Especificacion",
                table: "ProductoTbl",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AreaTbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaTbl", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductoTbl_Especificacion",
                table: "ProductoTbl",
                column: "Especificacion");
        }
    }
}
