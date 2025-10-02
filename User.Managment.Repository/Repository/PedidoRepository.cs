// <copyright file="PedidoRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Data;
using User.Managment.Data.Models.Ventas;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        private readonly ApplicationDbContext _db;

        public PedidoRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<Pedido> UpdateAsync(Pedido entity)
        {
            _db.PedidoTbl.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Pedido>> UpdateRangesAsync(List<Pedido> entities)
        {
            _db.PedidoTbl.UpdateRange(entities);
            await _db.SaveChangesAsync();
            return entities;
        }
    }
}