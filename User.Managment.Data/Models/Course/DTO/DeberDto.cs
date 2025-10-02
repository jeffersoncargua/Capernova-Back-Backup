// <copyright file="DeberDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Course.DTO
{
    public class DeberDto
    {
        public int Id { get; set; }

        [Required]
        public string? Titulo { get; set; }

        [Required]
        public string? Detalle { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}