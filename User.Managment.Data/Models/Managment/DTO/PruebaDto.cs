using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Managment.DTO
{
    public class PruebaDto
    {
        public string Id { get; set; }
        public string Titulo { get; set; } //Titulo de la prueba
        public string Detalle { get; set; } //Descripcion de lo que trata la prueba
        public string TestURL { get; set; } //En esta seccion se subira el link o enlace de la prueba
        public string Estado { get; set; } //Aquí se subira el estado de la prueba que puede ser Pendiente o Realizado
        public double Calificacion { get; set; } //Aquí se almacena la calificacion de la prueba
    }
}
