using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Ventas
{
    [Index(nameof(Venta.Emision))]
    [Index(nameof(Venta.UserId))]
    [Index(nameof(Venta.LastName))]
    [Index(nameof(Venta.Phone))]
    public class Venta
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public string TransaccionId { get; set; }
        public DateTime Emision { get; set; }
        public double Total { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Estado { get; set; }

    }
}
