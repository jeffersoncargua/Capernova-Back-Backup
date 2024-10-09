using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Ventas
{
    public class Cliente
    {
        public string? Id { get; set; } //cedula

        public string? Name { get; set; }

        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? DirectionMain { get; set; }
        public string? DirectionSec { get; set; }
    }
}
