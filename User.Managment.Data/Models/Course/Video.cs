// <copyright file="Video.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Managment.Data.Models.Course
{
    public class Video
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Titulo { get; set; }

        public string? VideoUrl { get; set; }

        public int? OrdenReproduccion { get; set; }

        [Required]
        public int CapituloId { get; set; }

        [ForeignKey("CapituloId")]
        public Capitulo? Capitulo { get; set; }
    }
}