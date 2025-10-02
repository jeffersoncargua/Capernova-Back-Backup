// <copyright file="20240828164815_AlterVentaTbl.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AlterVentaTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VentaTbl_AspNetUsers_UserId",
                table: "VentaTbl");

            migrationBuilder.DropForeignKey(
                name: "FK_VentaTbl_Cliente_ClienteId",
                table: "VentaTbl");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_VentaTbl_ClienteId",
                table: "VentaTbl");

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
                name: "ClienteId",
                table: "VentaTbl");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VentaTbl",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "VentaTbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "VentaTbl",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "VentaTbl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "VentaTbl",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2a346fb8-12b2-4c23-aa11-0d41673e0837", "4", "Teacher", "TEACHER" },
                    { "3f45430d-cd35-486a-a23b-2e17ca27967c", "2", "User", "USER" },
                    { "66dc0cb7-827d-4409-8002-fab756f843fd", "1", "Admin", "ADMIN" },
                    { "809f5a24-8575-492b-894e-446f6872ce6e", "5", "Secretary", "SECRETARY" },
                    { "b0801c74-76f8-4587-aa36-5a644f529670", "3", "Student", "STUDENT" },
                });

            migrationBuilder.CreateIndex(
                name: "IX_VentaTbl_LastName",
                table: "VentaTbl",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_VentaTbl_Phone",
                table: "VentaTbl",
                column: "Phone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VentaTbl_LastName",
                table: "VentaTbl");

            migrationBuilder.DropIndex(
                name: "IX_VentaTbl_Phone",
                table: "VentaTbl");

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

            migrationBuilder.DropColumn(
                name: "Email",
                table: "VentaTbl");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "VentaTbl");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "VentaTbl");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "VentaTbl");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VentaTbl",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ClienteId",
                table: "VentaTbl",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DirectionMain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectionSec = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
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
                    { "fe64a82b-8583-42b1-9c24-260f852b60e3", "5", "Secretary", "SECRETARY" },
                });

            migrationBuilder.CreateIndex(
                name: "IX_VentaTbl_ClienteId",
                table: "VentaTbl",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_VentaTbl_AspNetUsers_UserId",
                table: "VentaTbl",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VentaTbl_Cliente_ClienteId",
                table: "VentaTbl",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id");
        }
    }
}