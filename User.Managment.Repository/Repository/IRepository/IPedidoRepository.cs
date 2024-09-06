using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Data.Models.Ventas;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> UpdateAsync(Pedido entity);
        Task<List<Pedido>> UpdateRangesAsync(List<Pedido> entities);
    }
}

