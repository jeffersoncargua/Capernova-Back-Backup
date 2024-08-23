using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Data;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class ProductoRepositoy : Repository<Producto>, IProductoRepositoy
    {
        private readonly ApplicationDbContext _db;

        public ProductoRepositoy(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Producto> UpdateAsync(Producto entity)
        {
            _db.ProductoTbl.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Producto>> UpdateRangesAsync(List<Producto> entities)
        {
            _db.ProductoTbl.UpdateRange(entities);
            await _db.SaveChangesAsync();
            return entities;
        }
    }
}
