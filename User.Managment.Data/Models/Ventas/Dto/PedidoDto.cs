using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Ventas.Dto
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public DateTime Emision { get; set; }
        public string? Productos { get; set; }
        public int VentaId { get; set; }
        public string DirectionMain { get; set; }
        public string DirectionSec { get; set; }
        public string? Estado { get; set; }
    }
}
