// <copyright file="20240805225832_alterCourseTblAddFolderId.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class alterCourseTblAddFolderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38440e89-2e81-4c48-9eb2-a32356eb377a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48986b37-36a5-46ac-a7ac-32c895e02f1d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60514743-ab06-44e2-8bf5-5544b00885a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9da60f83-cc66-4222-a293-1278327cd86c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb79cc4a-9dd0-4bbc-8767-b42225091548");

            migrationBuilder.AddColumn<string>(
                name: "FolderId",
                table: "CourseTbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "219ad934-0d25-4021-9392-a6f155d94a38", "3", "Student", "STUDENT" },
                    { "2ac1e30f-e6bf-44bd-a4c3-5d22d0e26b9d", "5", "Secretary", "SECRETARY" },
                    { "65189451-c8c2-46bc-8b1d-5aba0490223e", "4", "Teacher", "TEACHER" },
                    { "7b2e14c0-f5f2-44c9-bde5-953cbbc9dbbb", "1", "Admin", "ADMIN" },
                    { "f8a78efa-cc5b-4a62-8d21-bcfbc63367c4", "2", "User", "USER" },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "CourseTbl");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38440e89-2e81-4c48-9eb2-a32356eb377a", "5", "Secretary", "SECRETARY" },
                    { "48986b37-36a5-46ac-a7ac-32c895e02f1d", "2", "User", "USER" },
                    { "60514743-ab06-44e2-8bf5-5544b00885a9", "3", "Student", "STUDENT" },
                    { "9da60f83-cc66-4222-a293-1278327cd86c", "1", "Admin", "ADMIN" },
                    { "cb79cc4a-9dd0-4bbc-8767-b42225091548", "4", "Teacher", "TEACHER" },
                });
        }
    }
}