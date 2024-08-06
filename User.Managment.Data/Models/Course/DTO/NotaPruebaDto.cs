using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Course.DTO
{
    public class NotaPruebaDto
    {
        public int Id { get; set; }
        public string? Estado { get; set; }
        public string? Observacion { get; set; }
        public string? FileUrl { get; set; }
        public double? Calificacion { get; set; }
        [Required]
        public string StudentId { get; set; }
        [Required]
        public int DeberId { get; set; }
    }
}
