using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.Ventas.Dto;

namespace User.Managment.Data.Models.PaypalOrder
{
    public class ConfirmOrder
    {
        public string? IdentifierName { get; set; }
        public string? Productos { get; set; }
        public string? Total { get; set; }
        public string? Orden { get; set; }
        //public string Token { get; set; }
    }
}
