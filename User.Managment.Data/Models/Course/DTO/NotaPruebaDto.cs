// <copyright file="NotaPruebaDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Course.DTO
{
    public class NotaPruebaDto
    {
        public int Id { get; set; }

        public string? Estado { get; set; }

        public string? Observacion { get; set; }

        public double? Calificacion { get; set; }

        [Required]
        public string? StudentId { get; set; }

        [Required]
        public int PruebaId { get; set; }
    }
}