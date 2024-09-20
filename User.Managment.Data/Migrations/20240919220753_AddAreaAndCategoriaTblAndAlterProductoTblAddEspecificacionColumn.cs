using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AddAreaAndCategoriaTblAndAlterProductoTblAddEspecificacionColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

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

            migrationBuilder.CreateTable(
                name: "CategoriaTbl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaTbl", x => x.Id);
                });


            migrationBuilder.CreateIndex(
                name: "IX_ProductoTbl_Especificacion",
                table: "ProductoTbl",
                column: "Especificacion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaTbl");

            migrationBuilder.DropTable(
                name: "CategoriaTbl");

            migrationBuilder.DropIndex(
                name: "IX_ProductoTbl_Especificacion",
                table: "ProductoTbl");

            migrationBuilder.DropColumn(
                name: "Especificacion",
                table: "ProductoTbl");

        }
    }
}
