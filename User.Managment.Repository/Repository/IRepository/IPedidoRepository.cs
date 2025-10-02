// <copyright file="IPedidoRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.Ventas;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> UpdateAsync(Pedido entity);

        Task<List<Pedido>> UpdateRangesAsync(List<Pedido> entities);
    }
}