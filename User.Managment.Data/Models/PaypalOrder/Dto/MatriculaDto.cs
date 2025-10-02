// <copyright file="MatriculaDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.PaypalOrder.Dto
{
    public class MatriculaDto
    {
        public int? Id { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        public string? EstudianteId { get; set; }

        public DateTime? FechaInscripcion { get; set; }

        public bool? IsActive { get; set; }

        public string? Estado { get; set; }

        public double? NotaFinal { get; set; }
    }
}