using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.Student;

namespace User.Managment.Data.Models.Course
{
    public class EstudianteVideo
    {
        [Key]
        public int Id { get; set; }
        public string? Estado { get; set; }
        [Required]
        public int VideoId { get; set; }
        [ForeignKey("VideoId")]
        public Video? Video { get; set; }
        [Required]
        public string StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student.Student? Student { get; set; }
    }
}
