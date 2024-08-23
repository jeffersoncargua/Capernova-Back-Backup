using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Student;

namespace User.Managment.Data.Models.PaypalOrder
{
    public class Matricula
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CursoId { get; set; }
        [ForeignKey("CursoId")]
        public Course.Course Curso{ get; set; }
        [Required]
        public string EstudianteId { get; set; }
        [ForeignKey("EstudianteId")]
        public Student.Student Estudiante { get; set; }
        public bool? IsActive { get; set; }
        public string? Estado { get; set; }
        public double? NotaFinal { get; set; }

    }
}
