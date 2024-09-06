using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Data;
using User.Managment.Data.Models.PaypalOrder;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class MatriculaRepository : Repository<Matricula>, IMatriculaRepository
    {
        private readonly ApplicationDbContext _db;

        public MatriculaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Matricula> UpdateAsync(Matricula entity)
        {
            _db.MatriculaTbl.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Matricula>> UpdateRangesAsync(List<Matricula> entities)
        {
            _db.MatriculaTbl.UpdateRange(entities);
            await _db.SaveChangesAsync();
            return entities;
        }
    }
}
