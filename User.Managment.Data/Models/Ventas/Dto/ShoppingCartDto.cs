using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Ventas.Dto
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Titulo { get; set; } //Es el nombre del producto
        public string? Imagen { get; set; } //Es el nombre del producto
        public int Cantidad { get; set; } 
        public double Precio { get; set; }
        public string? Tipo { get; set; } // Permite saber si el producto es de tipo curso o producto
        

    }
}
