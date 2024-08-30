using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Ventas
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Emision { get; set; }
        public string? Productos { get; set; }
        [Required]
        public int VentaId { get; set; }
        [ForeignKey("VentaId")]
        public Venta? Venta { get; set; }
        public string DirectionMain { get; set; }
        public string DirectionSec { get; set; }
    }
}
