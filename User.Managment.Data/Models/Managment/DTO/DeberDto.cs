using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Managment.DTO
{
    public class DeberDto
    {
        public string Id { get; set; }
        public string Titulo { get; set; } 
        public string Detalle { get; set; } //Aqui va el detalle sobre que trata el deber
        public  string Estado { get; set; } //Aqui va el estado del deber que puede ser: Por entregar o entregado
        public string? FileUrl { get; set; } //Aqui se va a subir la direccion del archivo  del deber
        public double? Calificacion { get; set; } //Aqui va la calificacion del deber
    }
}
