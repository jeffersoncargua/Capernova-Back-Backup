// <copyright file="NotaDeberDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Course.DTO
{
    public class NotaDeberDto
    {
        public int Id { get; set; }

        public string? Estado { get; set; }

        public string? Observacion { get; set; }

        public string? FileUrl { get; set; }

        public double? Calificacion { get; set; }

        [Required]
        public string? StudentId { get; set; }

        [Required]
        public int DeberId { get; set; }
    }
}