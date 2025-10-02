// <copyright file="PruebaDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Course.DTO
{
    public class PruebaDto
    {
        public int Id { get; set; }

        [Required]
        public string? Titulo { get; set; }

        [Required]
        public string? Detalle { get; set; }

        public string? Test { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}