using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.ProductosServicios
{
    [Index(nameof(Producto.Codigo), IsUnique = true)]
    [Index(nameof(Producto.Tipo))]
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]        
        public string Codigo { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public int Cantidad { get; set; }
        public string? ImagenUrl { get; set; }
        public string? Detalle { get; set; }
        [Required]
        public double Precio { get; set; }
        public string Tipo { get; set; }
    }
}

