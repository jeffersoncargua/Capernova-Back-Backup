using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Managment.DTO
{
    public class CapituloDto
    {
        public string Codigo { get; set; } //Identificador del capitulo de un determinado Curso por ejemplo un curso de MakeUp ("CN-Capitulo1-Makeup")
        public string Titulo { get; set; }
        public List<VideoDto> Videos { get; set; }

    }
}
