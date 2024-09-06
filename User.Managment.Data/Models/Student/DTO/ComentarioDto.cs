using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Student.DTO
{
    public class ComentarioDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        
        public string FeedBack { get; set; }

        public string Titulo { get; set; }
    }
}
