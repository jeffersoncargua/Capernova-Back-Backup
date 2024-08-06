using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Course.DTO
{
    public class PruebaDto
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Detalle { get; set; }
        public string? Test { get; set; }
        [Required]
        public int CourseId { get; set; }
    }
}
