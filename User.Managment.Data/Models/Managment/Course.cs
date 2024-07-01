using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public bool isActive { get; set; }
        public string Capitulos { get; set; } //Este cmapo permite guardar el listado de vdeos
    }
}
