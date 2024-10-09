using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.ProductosServicios.Dto
{
    public class ProductoDto
    {
        public int Id { get; set; }
        [Required]
        public string? Codigo { get; set; }
        [Required]
        public string? Titulo { get; set; }
        [Required]
        public int Cantidad { get; set; }
        public int? CategoriaId { get; set; }
        public string? ImagenUrl { get; set; }
        public string? Especificacion { get; set; }
        public string? Detalle { get; set; }
        [Required]
        public double Precio { get; set; }
        public string? Tipo { get; set; }
    }
}
