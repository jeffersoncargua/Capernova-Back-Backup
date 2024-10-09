using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.PaypalOrder
{
    /// <summary>
    /// Esta clase permite recibir los parametros necesarios para generar la informacion del carrito de compras, ventas y matriculacion en el caso de adquirir cursos 
    /// </summary>
    public class Order
    {
        public string? Productos { get; set; }
        public string? Total { get; set; }
    }


    public class OrderDto
    {
        public string? Id { get; set; }
        public string? Codigo { get; set; }
        public string? Titulo { get; set; }
        public string? Cantidad { get; set; }
        //public string Imagen { get; set; }
        public string? Detalle { get; set; }
        public double Precio { get; set; }
        public string? Tipo { get; set; }

    }

}
