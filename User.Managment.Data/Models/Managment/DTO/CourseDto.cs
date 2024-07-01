using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Managment.DTO
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public double Price { get; set; }
        public bool isActive { get; set; } = false;
        public List<CapituloDto> CapituloList { get; set; }
    }
}
