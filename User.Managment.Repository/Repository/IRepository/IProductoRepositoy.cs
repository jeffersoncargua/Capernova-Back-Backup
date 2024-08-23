using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.ProductosServicios;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IProductoRepositoy : IRepository<Producto>
    {
        Task<Producto> UpdateAsync(Producto entity);
        Task<List<Producto>> UpdateRangesAsync(List<Producto> entities);
    }
}
