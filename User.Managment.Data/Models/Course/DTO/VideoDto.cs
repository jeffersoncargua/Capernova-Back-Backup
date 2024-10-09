using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Course.DTO
{
    public class VideoDto
    {
        public int Id { get; set; }
        [Required]
        public string? Titulo { get; set; }
        public string? VideoUrl { get; set; }
        public int? OrdenReproduccion { get; set; }
        [Required]
        public int CapituloId { get; set; }
    }
}
