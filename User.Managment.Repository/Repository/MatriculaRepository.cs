// <copyright file="MatriculaRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Data;
using User.Managment.Data.Models.PaypalOrder;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class MatriculaRepository : Repository<Matricula>, IMatriculaRepository
    {
        private readonly ApplicationDbContext _db;

        public MatriculaRepository(ApplicationDbContext db)
            : base(db)
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