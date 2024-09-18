using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AlterVentaTblAddTransaccionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransaccionId",
                table: "VentaTbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransaccionId",
                table: "VentaTbl");

        }
    }
}
