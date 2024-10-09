using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.ProductosServicios;

namespace User.Managment.Data.Models.Course
{
    [Index(nameof(Course.Codigo))]
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Codigo { get; set; }
        [Required]
        public string? Titulo { get; set; }
        [Required]
        public string? Detalle  { get; set; }
        [Required]
        public string? ImagenUrl { get; set; }
        [Required]
        public double Precio { get; set; }
        public string? TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }
        public string? FolderId { get; set; } // Permite almacenar informacion en la carpeta asiganada que en este caso sera el nombre o titulo del Curso

        public string? BibliotecaUrl { get; set; }
        public string? ClaseUrl { get; set; }

    }
}
