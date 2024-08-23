using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.PaypalOrder.Dto
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        [Required]

        public int ProductoId { get; set; }
        [Required]
        public int FacturaId { get; set; }
        [Required]
        public double Cantidad { get; set; }
    }
}
