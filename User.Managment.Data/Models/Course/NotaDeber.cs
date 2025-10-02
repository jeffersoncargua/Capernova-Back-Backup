// <copyright file="NotaDeber.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Managment.Data.Models.Course
{
    public class NotaDeber
    {
        [Key]
        public int Id { get; set; }

        public string? Estado { get; set; }

        public string? Observacion { get; set; }

        public string? FileUrl { get; set; }

        public double? Calificacion { get; set; }

        [Required]
        public string? StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student.Student? Student { get; set; }

        [Required]
        public int DeberId { get; set; }

        [ForeignKey("DeberId")]
        public Deber? Deber { get; set; }
    }
}