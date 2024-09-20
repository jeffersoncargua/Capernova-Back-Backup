using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.ProductosServicios
{
    [Index(nameof(Producto.Codigo), IsUnique = true)]
    [Index(nameof(Producto.Tipo))]
    [Index(nameof(Producto.CategoriaId))]
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
        public int? CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }
        public string? ImagenUrl { get; set; }
        public string? Detalle { get; set; }
        [Required]
        public double Precio { get; set; }
        public string Tipo { get; set; }
    }
}

