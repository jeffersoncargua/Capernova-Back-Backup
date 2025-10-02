// <copyright file="20240822224048_AddIndexToCourseToCodigo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AddIndexToCourseToCodigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15d7fffd-7399-497a-8ddd-3ae7cf9cf645");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "380a255f-8abe-49c3-9530-6992d8d2f78e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b22d7c21-0f95-4e9d-91ee-c9285fbe2429");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c949d71e-71a4-45e5-9df1-a1edab0e45cc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed50890c-5c9c-41d3-9fd9-204e6e01b973");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "CourseTbl",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4963f4ca-3b0e-46ac-be34-606e2544d265", "5", "Secretary", "SECRETARY" },
                    { "85093123-61ee-4ba3-b4c1-da9cc7ef9d86", "2", "User", "USER" },
                    { "d280250d-77c0-4efe-977e-364f14de445f", "3", "Student", "STUDENT" },
                    { "e9468390-25a1-4115-a182-6e4fc789289a", "4", "Teacher", "TEACHER" },
                    { "fb5f07f1-06d3-45d2-ab96-6cbcb0da117e", "1", "Admin", "ADMIN" },
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTbl_Codigo",
                table: "CourseTbl",
                column: "Codigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourseTbl_Codigo",
                table: "CourseTbl");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4963f4ca-3b0e-46ac-be34-606e2544d265");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85093123-61ee-4ba3-b4c1-da9cc7ef9d86");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d280250d-77c0-4efe-977e-364f14de445f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9468390-25a1-4115-a182-6e4fc789289a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb5f07f1-06d3-45d2-ab96-6cbcb0da117e");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "CourseTbl",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15d7fffd-7399-497a-8ddd-3ae7cf9cf645", "4", "Teacher", "TEACHER" },
                    { "380a255f-8abe-49c3-9530-6992d8d2f78e", "5", "Secretary", "SECRETARY" },
                    { "b22d7c21-0f95-4e9d-91ee-c9285fbe2429", "1", "Admin", "ADMIN" },
                    { "c949d71e-71a4-45e5-9df1-a1edab0e45cc", "3", "Student", "STUDENT" },
                    { "ed50890c-5c9c-41d3-9fd9-204e6e01b973", "2", "User", "USER" },
                });
        }
    }
}