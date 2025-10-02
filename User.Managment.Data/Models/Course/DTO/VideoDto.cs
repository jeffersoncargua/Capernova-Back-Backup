// <copyright file="VideoDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Course.DTO
{
    public class VideoDto
    {
        public int Id { get; set; }

        [Required]
        public string? Titulo { get; set; }

        public string? VideoUrl { get; set; }

        public int? OrdenReproduccion { get; set; }

        [Required]
        public int CapituloId { get; set; }
    }
}