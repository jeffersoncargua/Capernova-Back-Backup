using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.Managment.DTO;

namespace User.Managment.Data.Models.Student.DTO
{
    public class StudentDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Photo { get; set; }
        //public List<CourseDto>? Courses { get; set; }
    }
}
