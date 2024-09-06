using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Student
{
    public class Comentario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        [Required]
        public string FeedBack { get; set; }
        public string Titulo { get; set; }

    }
}
