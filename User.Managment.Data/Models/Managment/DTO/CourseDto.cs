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
        public bool? IsActive { get; set; } = false;

        public string? State { get; set; } //permite ver si el esatdo del curso es 'En progreso' , 'Aprobado' , 'Reprobado'.
        public string? TeacherId { get; set; }
        public List<CapituloDto>? CapituloList { get; set; } // Aqui va la lista de capitulos con los videos
        public List<DeberDto>? Deberes { get; set; } //Aqui van los deberes que se van a cargar por parte de los profes
        public List<PruebaDto>? Pruebas { get; set; } //Aqui van las pruebas que se van a cargar por parte de los profes
        public double? NotaFinal { get; set; }
    }
}
