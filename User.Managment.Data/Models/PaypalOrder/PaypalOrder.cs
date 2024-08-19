using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.PaypalOrder
{
    public class PaypalOrder
    {
            public string intent { get; set; }
            public List<PurchaseUnit> purchase_units { get; set; }
            public ApplicationContext application_context { get; set; }
    }

    public class ItemTotal
    {
        public string currency_code { get; set; }
        public string value { get; set; }

    }

    public class Breakdown
    {
        public ItemTotal item_total{ get; set; }
    }



    public class Amount
    {
        public string currency_code { get; set; } //Es la moneda con la que se va a apagar
        public string value { get; set; } //Es el valor que se va a pagar
        public Breakdown breakdown { get; set; }
    }

    public class UnitAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string sku { get; set; } // sirve para la identificacion del produto que se adquiere
        public string quantity { get; set; }
        public UnitAmount unit_amount { get; set; }
    }

    public class ApplicationContext
    {
        public string brand_name { get; set; } //Es el nombre del sitio Web al que se le va a pagar
        
        public string? payment_method_preference { get; set; } // PERMITE AL USUARIO PAGAR DE CUALQUIER MANERA SIN NECESIDAD DE TENER CUENTA PAYPAL

        public string? payment_method_selected { get; set; } // es un parámetro neceseario para el formato para realizar las transacciones con Paypal tanto en pago con paypal mismo o tarjeta de credito o debito
        public string? locale { get; set; } //permite al usuario verificar su region de compra
        public string landing_page { get; set; } //
        public string user_action { get; set; } //Es la acccion del pago
        public string return_url { get; set; } //Permite redirigir a una pagina cuando se realice con exito la compra
        public string cancel_url { get; set; } //Permite redirigir a una pagina en caso de cancelacion de la compra
    }

    public class PurchaseUnit
    {
        public Amount amount { get; set; }
        public string description { get; set; }
        public List<Item> items { get; set; }
        
    }
   
}
