// <copyright file="IVentaRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.Ventas;
using User.Managment.Data.Models.Ventas.Dto;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IVentaRepository : IRepository<Venta>
    {
        Task<ResponseDto> GetAllVentasAsync(string? search, string start, string end);

        Task<ResponseDto> GetAllPedidosAsync(string? search, string start, string end);

        Task<ResponseDto> GetShoppingCartAsync(int ventaId);

        Task<ResponseDto> DeleteVentaAsync(int id);

        Task<ResponseDto> UpdateVentaAsync(int id);

        Task<ResponseDto> UpdatePedidoAsync(int id, PedidoDto pedidoDto);

        Task<ResponseDto> DeletePedidoAsync(int id);
    }
}