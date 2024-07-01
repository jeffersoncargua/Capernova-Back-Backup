using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Managment.DTO
{
    public class VideoDto
    {
        public string Codigo { get; set; } //Es el identificador del video para un determinado curso
        public string Titulo { get; set; }
        public string VideoUrl { get; set; }
        public int OrdenReproduccion { get; set; }
        public bool Visto { get; set; } = false;

    }
}
