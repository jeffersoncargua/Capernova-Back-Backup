using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Course.DTO
{
    public class EstudianteVideoDto
    {
        public int Id { get; set; }
        public string? Estado { get; set; }
        [Required]
        public int VideoId { get; set; }

        [Required]
        public int CursoId { get; set; }
        [Required]
        public string? StudentId { get; set; }
    }
}
