using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Course
{
    public class NotaPrueba
    {
        [Key]
        public int Id { get; set; }
        public string? Estado { get; set; }
        public string? Observacion { get; set; }
        public double? Calificacion { get; set; }
        [Required]
        public string StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student.Student? Student { get; set; }
        [Required]
        public int PruebaId { get; set; }
        [ForeignKey("PruebaId")]
        public Prueba? Prueba { get; set; }
    }
}
