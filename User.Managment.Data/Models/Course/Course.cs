// <copyright file="Course.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using User.Managment.Data.Models.Managment;

namespace User.Managment.Data.Models.Course
{
    [Index(nameof(Codigo))]
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Codigo { get; set; }

        [Required]
        public string? Titulo { get; set; }

        [Required]
        public string? Detalle { get; set; }

        [Required]
        public string? ImagenUrl { get; set; }

        [Required]
        public double Precio { get; set; }

        public string? TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }

        public string? FolderId { get; set; } // Permite almacenar informacion en la carpeta asiganada que en este caso sera el nombre o titulo del Curso

        public string? BibliotecaUrl { get; set; }

        public string? ClaseUrl { get; set; }
    }
}