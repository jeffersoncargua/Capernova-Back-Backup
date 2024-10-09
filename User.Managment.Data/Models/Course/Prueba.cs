using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Course
{
    public class Prueba
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Titulo { get; set; }
        [Required]
        public string? Detalle { get; set; }
        public string? Test { get; set; }
        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }
}
