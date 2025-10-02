// <copyright file="20240926215406_AlterCourseTblAddBibliotecaAndLiveAndAlterMatriculaAddFechaInscrip.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Managment.Data.Migrations
{
    public partial class AlterCourseTblAddBibliotecaAndLiveAndAlterMatriculaAddFechaInscrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInscripcion",
                table: "MatriculaTbl",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BibliotecaUrl",
                table: "CourseTbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaseUrl",
                table: "CourseTbl",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaInscripcion",
                table: "MatriculaTbl");

            migrationBuilder.DropColumn(
                name: "BibliotecaUrl",
                table: "CourseTbl");

            migrationBuilder.DropColumn(
                name: "ClaseUrl",
                table: "CourseTbl");
        }
    }
}