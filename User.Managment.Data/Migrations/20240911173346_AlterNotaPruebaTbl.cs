// <copyright file="20240911173346_AlterNotaPruebaTbl.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AlterNotaPruebaTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotaPruebaTbl_PruebaTbl_DeberId",
                table: "NotaPruebaTbl");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "NotaPruebaTbl");

            migrationBuilder.RenameColumn(
                name: "DeberId",
                table: "NotaPruebaTbl",
                newName: "PruebaId");

            migrationBuilder.RenameIndex(
                name: "IX_NotaPruebaTbl_DeberId",
                table: "NotaPruebaTbl",
                newName: "IX_NotaPruebaTbl_PruebaId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotaPruebaTbl_PruebaTbl_PruebaId",
                table: "NotaPruebaTbl",
                column: "PruebaId",
                principalTable: "PruebaTbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotaPruebaTbl_PruebaTbl_PruebaId",
                table: "NotaPruebaTbl");

            migrationBuilder.RenameColumn(
                name: "PruebaId",
                table: "NotaPruebaTbl",
                newName: "DeberId");

            migrationBuilder.RenameIndex(
                name: "IX_NotaPruebaTbl_PruebaId",
                table: "NotaPruebaTbl",
                newName: "IX_NotaPruebaTbl_DeberId");

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "NotaPruebaTbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NotaPruebaTbl_PruebaTbl_DeberId",
                table: "NotaPruebaTbl",
                column: "DeberId",
                principalTable: "PruebaTbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}