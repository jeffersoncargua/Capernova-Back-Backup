using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AlterVentaTblAddEstado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "VentaTbl",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {          
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "VentaTbl");
        }
    }
}
