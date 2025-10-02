// <copyright file="EstudianteVideoDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Course.DTO
{
    public class EstudianteVideoDto
    {
        public int Id { get; set; }

        public string? Estado { get; set; }

        [Required]
        public int VideoId { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        public string? StudentId { get; set; }
    }
}