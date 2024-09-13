

using User.Managment.Data.Data;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class DeberRepository : Repository<Deber>, IDeberRepository
    {
        private readonly ApplicationDbContext _db;

        public DeberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Deber> UpdateAsync(Deber entity)
        {
            _db.DeberTbl.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Deber>> UpdateRangesAsync(List<Deber> entities)
        {
            _db.DeberTbl.UpdateRange(entities);
            await _db.SaveChangesAsync();
            return entities;
        }
    }
}
