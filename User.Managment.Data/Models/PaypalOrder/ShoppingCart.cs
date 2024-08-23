using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.ProductosServicios;

namespace User.Managment.Data.Models.PaypalOrder
{
    /// <summary>
    /// Permite almacenar los productos que se adquieron por parte del cliente en la base de datos
    /// </summary>
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }
        [Required]
        public int FacturaId { get; set; }
        [ForeignKey("FacturaId")]
        public Facturacion? Facturacion { get; set; }
        [Required]
        public double Cantidad { get; set; }
    }

}
