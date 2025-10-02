// <copyright file="CourseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Course.DTO
{
    public class CourseDto
    {
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

        public string? FolderId { get; set; }

        public string? BibliotecaUrl { get; set; }

        public string? ClaseUrl { get; set; }

        public int? CategoriaId { get; set; }
    }
}