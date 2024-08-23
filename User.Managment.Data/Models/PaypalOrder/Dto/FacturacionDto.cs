using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.PaypalOrder.Dto
{
    public class FacturacionDto
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime? FechaVenta { get; set; }
        public double Total { get; set; }
    }
}
