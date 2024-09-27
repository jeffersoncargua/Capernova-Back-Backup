using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.Managment;

namespace User.Managment.Data.Models.Course.DTO
{
    public class CourseDto
    {
        public int Id { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Detalle { get; set; }
        [Required]
        public string ImagenUrl { get; set; }
        [Required]
        public double Precio { get; set; }
        public string? TeacherId { get; set; }
        public string? FolderId { get; set; }

        public string? BibliotecaUrl { get; set; }
        public string? ClaseUrl { get; set; }

        public int CategoriaId { get; set; }
    }
}
