// <copyright file="VentaController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using Microsoft.AspNetCore.Mvc;
using User.Managment.Data.Models.Ventas.Dto;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaRepository _repoVenta;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public VentaController(IVentaRepository repoVenta, IMapper mapper)
        {
            _repoVenta = repoVenta;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [Route("getAllVentas")]
        public async Task<ActionResult<ApiResponse>> GetAllVentas([FromQuery] string? search, [FromQuery] string start, [FromQuery] string end)
        {
            var result = await _repoVenta.GetAllVentasAsync(search, start, end);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getShoppingCart")]
        public async Task<ActionResult<ApiResponse>> GetShoppingCart([FromQuery] int ventaId)
        {
            var result = await _repoVenta.GetShoppingCartAsync(ventaId);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete("deleteVenta/{id:int}", Name = "deleteVenta")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteVenta(int id)
        {
            var result = await _repoVenta.DeleteVentaAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet]
        [Route("getAllPedidos")]
        public async Task<ActionResult<ApiResponse>> GetAllPedidos([FromQuery] string? search, [FromQuery] string start, [FromQuery] string end)
        {
            var result = await _repoVenta.GetAllPedidosAsync(search, start, end);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateVenta/{id:int}", Name = "updateVenta")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateVenta(int id)
        {
            var result = await _repoVenta.UpdateVentaAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updatePedido/{id:int}", Name = "updatePedido")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdatePedido(int id, [FromBody] PedidoDto pedidoDto)
        {
            var result = await _repoVenta.UpdatePedidoAsync(id, pedidoDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete("deletePedido/{id:int}", Name = "deletePedido")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeletePedido(int id)
        {
            var result = await _repoVenta.DeletePedidoAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}