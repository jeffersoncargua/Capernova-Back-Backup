using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.PaypalOrder
{
    public class PaypalOrder
    {
        
            public string? intent { get; set; }
            public List<PurchaseUnit>? purchase_units { get; set; }
            public ApplicationContext? application_context { get; set; }
    }

    public class Amount
    {
        public string? currency_code { get; set; } //Es la moneda con la que se va a apagar
        public string? value { get; set; } //Es el valor que se va a pagar
    }

    public class ApplicationContext
    {
        public string? brand_name { get; set; } //Es el nombre del sitio Web al que se le va a pagar
        public string? landing_page { get; set; } //
        public string? user_action { get; set; } //Es la acccion del pago
        public string? return_url { get; set; } //Permite redirigir a una pagina cuando se realice con exito la compra
        public string? cancel_url { get; set; } //Permite redirigir a una pagina en caso de cancelacion de la compra
    }

    public class PurchaseUnit
    {
        public Amount? amount { get; set; }
        public string? description { get; set; }
    }
   
}
