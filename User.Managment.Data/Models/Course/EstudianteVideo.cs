// <copyright file="EstudianteVideo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Managment.Data.Models.Course
{
    [Index(nameof(CursoId))]
    public class EstudianteVideo
    {
        [Key]
        public int Id { get; set; }

        public string? Estado { get; set; }

        [Required]
        public int VideoId { get; set; }

        [ForeignKey("VideoId")]
        public Video? Video { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        public string? StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student.Student? Student { get; set; }
    }
}