using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Managment
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Error, no puede excederse con la longitud del nombre del Curso")]
        public string Titulo { get; set; }
        [Required]
        public string Descripcion { get; set; }
        
        [Required]
        public double Price { get; set; }
        public bool? IsActive { get; set; }
        public string? State { get; set; } //Este campo permite saber si el curso ha sido aprobado o no
        public string? Capitulos { get; set; } //Este campo permite guardar el listado de vdeos
        
        public string? TeacherId { get; set; } //Este campo permite almacenar la id del profesor que va a impartir este curso
        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }
        public string? Deberes { get; set; } //Este campo permitira almacenar los deberes del estudiante
        public string? Pruebas { get; set; } //Este campo permitira almacenar los deberes del estudiante
        public double? NotaFinal { get; set; }
    }
}
