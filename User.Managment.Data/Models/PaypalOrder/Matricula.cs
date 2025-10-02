// <copyright file="Matricula.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Managment.Data.Models.PaypalOrder
{
    public class Matricula
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CursoId { get; set; }

        [ForeignKey("CursoId")]
        public Course.Course? Curso { get; set; }

        [Required]
        public string? EstudianteId { get; set; }

        [ForeignKey("EstudianteId")]
        public Student.Student? Estudiante { get; set; }

        public DateTime? FechaInscripcion { get; set; }

        public bool? IsActive { get; set; }

        public string? Estado { get; set; }

        public double? NotaFinal { get; set; }

        public string? CertificadoId { get; set; }
    }
}