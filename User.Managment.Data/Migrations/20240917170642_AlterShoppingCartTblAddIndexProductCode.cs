// <copyright file="20240917170642_AlterShoppingCartTblAddIndexProductCode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AlterShoppingCartTblAddIndexProductCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductoCode",
                table: "ShoppingCartTbl",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartTbl_ProductoCode",
                table: "ShoppingCartTbl",
                column: "ProductoCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartTbl_ProductoCode",
                table: "ShoppingCartTbl");

            migrationBuilder.AlterColumn<string>(
                name: "ProductoCode",
                table: "ShoppingCartTbl",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}