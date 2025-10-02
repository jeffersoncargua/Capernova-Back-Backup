// <copyright file="20240820220308_AddIndexProductoTipo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AddIndexProductoTipo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "ProductoTbl",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07b29794-c0b5-4989-ad17-dcda4abe775f", "1", "Admin", "ADMIN" },
                    { "2a4c397f-a0a7-4429-8863-f7647d52720f", "4", "Teacher", "TEACHER" },
                    { "6670cee8-cbfc-4f0f-97d4-f34c04d9434a", "5", "Secretary", "SECRETARY" },
                    { "6b556c2d-fa8d-4ac2-9934-66b95125fd0b", "2", "User", "USER" },
                    { "ca645d23-1cbf-475c-b761-ee45ed3d4e63", "3", "Student", "STUDENT" },
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductoTbl_Tipo",
                table: "ProductoTbl",
                column: "Tipo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductoTbl_Tipo",
                table: "ProductoTbl");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07b29794-c0b5-4989-ad17-dcda4abe775f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a4c397f-a0a7-4429-8863-f7647d52720f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6670cee8-cbfc-4f0f-97d4-f34c04d9434a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b556c2d-fa8d-4ac2-9934-66b95125fd0b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca645d23-1cbf-475c-b761-ee45ed3d4e63");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "ProductoTbl",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "48190452-554a-44e5-9673-6050d9205e58", "1", "Admin", "ADMIN" },
                    { "7e0812a6-feee-4585-8b64-9fe181d67d45", "5", "Secretary", "SECRETARY" },
                    { "9f68fefa-94ab-4bd1-9770-736d1eca66f4", "4", "Teacher", "TEACHER" },
                    { "b3b33d82-ffdc-4ec8-af14-b5aa79406fef", "2", "User", "USER" },
                    { "cbb2734f-d32b-4b77-9b22-fab36e758b18", "3", "Student", "STUDENT" },
                });
        }
    }
}