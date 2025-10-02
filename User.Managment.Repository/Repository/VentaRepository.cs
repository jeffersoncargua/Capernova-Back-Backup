// <copyright file="VentaRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Data.Models.Ventas;
using User.Managment.Data.Models.Ventas.Dto;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class VentaRepository : Repository<Venta>, IVentaRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IPedidoRepository _dbPedido;
        private readonly IProductoRepository _dbProducto;
        private readonly IMatriculaRepository _dbMatricula;

        // private readonly IMapper _mapper;
        protected ResponseDto _response;

        public VentaRepository(ApplicationDbContext db, IPedidoRepository dbPedido, IMatriculaRepository dbMatricula/*, IMapper mapper*/, IProductoRepository dbProducto)
            : base(db)
        {
            _db = db;
            _dbPedido = dbPedido;
            _dbMatricula = dbMatricula;

            // _mapper = mapper;
            _dbProducto = dbProducto;
            _response = new();
        }

        public async Task<ResponseDto> DeletePedidoAsync(int id)
        {
            try
            {
                var pedido = await _dbPedido.GetAsync(u => u.Id == id);
                if (pedido == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                }
                else
                {
                    await _dbPedido.RemoveAsync(pedido);

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "El pedido ha sido eliminado con éxito";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> DeleteVentaAsync(int id)
        {
            try
            {
                var venta = await this.GetAsync(u => u.Id == id, tracked: false);
                if (venta == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                }
                else
                {
                    await this.RemoveAsync(venta);

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "La venta ha sido eliminada con éxito";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllPedidosAsync(string? search, string start, string end)
        {
            try
            {
                if (!string.IsNullOrEmpty(search) && start != "null" && end != "null")
                {
                    DateTime startDate = JsonConvert.DeserializeObject<DateTime>(start);
                    DateTime endDate = JsonConvert.DeserializeObject<DateTime>(end);
                    var pedidosQuery = await _dbPedido.GetAllAsync(
                        u => (u.Venta!.LastName!.Contains(search)
                    || u.Venta.UserId!.Contains(search)
                    || u.Venta.TransaccionId!.ToLower().Contains(search))
                    && u.Emision >= startDate && u.Emision <= endDate, tracked: false, includeProperties: "Venta");
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de los pedidos";
                    _response.Result = pedidosQuery.OrderByDescending(x => x.Emision);
                    return _response;
                }
                else if (start != "null" && end != "null")
                {
                    var startDate = JsonConvert.DeserializeObject<DateTime>(start);
                    var endDate = JsonConvert.DeserializeObject<DateTime>(end);
                    var pedidosQuery = await _dbPedido.GetAllAsync(u => u.Emision >= startDate && u.Emision <= endDate, tracked: false, includeProperties: "Venta");
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de los pedidos";
                    _response.Result = pedidosQuery.OrderByDescending(x => x.Emision);
                    return _response;
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    var pedidosQuery = await _dbPedido.GetAllAsync(
                        u => u.Venta!.LastName!.Contains(search)
                    || u.Venta.UserId!.Contains(search)
                    || u.Venta.TransaccionId!.ToLower().Contains(search), tracked: false, includeProperties: "Venta");
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de los pedidos";
                    _response.Result = pedidosQuery.OrderByDescending(x => x.Emision);
                    return _response;
                }

                var pedidos = await _dbPedido.GetAllAsync(tracked: false, includeProperties: "Venta");

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la lista de las pedidos";
                _response.Result = pedidos.OrderByDescending(x => x.Emision);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllVentasAsync(string? search, string start, string end)
        {
            try
            {
                if (!string.IsNullOrEmpty(search) && start != "null" && end != "null")
                {
                    var startDate = JsonConvert.DeserializeObject<DateTime>(start);
                    var endDate = JsonConvert.DeserializeObject<DateTime>(end);
                    var ventasQuery = await _db.VentaTbl.Where(u => (u.UserId!.Contains(search)
                    || u.LastName!.ToLower().Contains(search)
                    || u.Email!.ToLower().Contains(search)
                    || u.TransaccionId!.ToLower().Contains(search))
                    && u.Emision >= startDate && u.Emision <= endDate).AsNoTracking().OrderByDescending(x => x.Emision).ToListAsync();
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de las Ventas";
                    _response.Result = ventasQuery;
                    return _response;
                }
                else if (start != "null" && end != "null")
                {
                    var startDate = JsonConvert.DeserializeObject<DateTime>(start);
                    var endDate = JsonConvert.DeserializeObject<DateTime>(end);
                    var ventasQuery = await _db.VentaTbl.Where(u => u.Emision >= startDate && u.Emision <= endDate).AsNoTracking().OrderByDescending(x => x.Emision).ToListAsync();
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de las Ventas";
                    _response.Result = ventasQuery;
                    return _response;
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    var ventasQuery = await _db.VentaTbl.Where(u => u.UserId!.Contains(search)
                    || u.LastName!.ToLower().Contains(search)
                    || u.Email!.ToLower().Contains(search)
                    || u.TransaccionId!.ToLower().Contains(search)).AsNoTracking().OrderByDescending(x => x.Emision).ToListAsync();
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de las Ventas";
                    _response.Result = ventasQuery;
                    return _response;
                }

                var ventas = await _db.VentaTbl.AsNoTracking().OrderByDescending(x => x.Emision).ToListAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la lista de las Ventas";
                _response.Result = ventas;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetShoppingCartAsync(int ventaId)
        {
            try
            {
                var cartList = await _db.ShoppingCartTbl.Where(u => u.VentaId == ventaId).ToListAsync();
                if (cartList != null)
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = $"Se ha obtenido la lista de los produtos pertenecientes a la venta {ventaId}";
                    _response.Result = cartList;
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = $"Error. No se obtuvo la lista de los produtos pertenecientes a la venta {ventaId}";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdatePedidoAsync(int id, PedidoDto pedidoDto)
        {
            try
            {
                var pedidoFromDb = await _dbPedido.GetAsync(u => u.Id == pedidoDto.Id, tracked: false);
                if (pedidoFromDb == null || pedidoDto == null || id != pedidoDto.Id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe";
                }
                else
                {
                    Pedido model = new()
                    {
                        Id = pedidoFromDb.Id,
                        Emision = pedidoFromDb.Emision,
                        Productos = pedidoFromDb.Productos,
                        VentaId = pedidoFromDb.VentaId,
                        DirectionMain = pedidoDto.DirectionMain,
                        DirectionSec = pedidoDto.DirectionSec,
                        Estado = pedidoDto.Estado,
                    };

                    _db.PedidoTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "El pedido ha sido actualizado con éxito";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateVentaAsync(int id)
        {
            try
            {
                var ventaExist = await this.GetAsync(u => u.Id == id, tracked: false);
                if (ventaExist == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se pudo realizar efectuar el reembolso, inténtelo nuevamente";
                    return _response;
                }
                else
                {
                    var shoppingFromDb = await _db.ShoppingCartTbl.AsNoTracking().Where(u => u.VentaId == ventaExist.Id).ToListAsync();
                    if (shoppingFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "No se pudo realizar efectuar el reembolso, inténtelo nuevamente";
                        return _response;
                    }
                    else
                    {
                        foreach (var itemCart in shoppingFromDb)
                        {
                            var producto = await _dbProducto.GetAsync(u => u.Codigo == itemCart.ProductoCode, tracked: false);
                            if (producto == null)
                            {
                                _response.IsSuccess = false;
                                _response.StatusCode = HttpStatusCode.BadRequest;
                                _response.Message = "No se pudo realizar efectuar el reembolso, inténtelo nuevamente";
                                return _response;
                            }
                            else if (producto.Tipo == "producto")
                            {
                                // Esta funcion permite actualizar el stock de los productos del tipo producto cuando se realiza un reembolso
                                Producto productoModel = new()
                                {
                                    Id = producto.Id,
                                    Codigo = producto.Codigo,
                                    Titulo = producto.Titulo,
                                    Cantidad = producto.Cantidad + itemCart.Cantidad,
                                    ImagenUrl = producto.ImagenUrl,
                                    Precio = producto.Precio,
                                    Tipo = producto.Tipo,
                                    Detalle = producto.Detalle,
                                };

                                _db.ProductoTbl.Update(productoModel);
                                await _db.SaveChangesAsync();
                            }
                            else if (producto.Tipo == "curso")
                            {
                                // Esta funcion permite eliminar la matricula de un usuario cuando se ha realizado el reembolso
                                var matriculaExist = await _dbMatricula.GetAsync(u => u.Curso!.Codigo == itemCart.ProductoCode && u.EstudianteId == ventaExist.UserId, tracked: false);
                                if (matriculaExist != null)
                                {
                                    await _dbMatricula.RemoveAsync(matriculaExist);
                                }
                            }
                        }
                    }

                    var pedidoFromDb = await _dbPedido.GetAsync(u => u.VentaId == id, tracked: false);
                    if (pedidoFromDb != null)
                    {
                        await _dbPedido.RemoveAsync(pedidoFromDb);
                        await _dbPedido.SaveAsync();
                    }

                    Venta updateVenta = new()
                    {
                        Id = ventaExist.Id,
                        TransaccionId = ventaExist.TransaccionId,
                        Total = ventaExist.Total,
                        UserId = ventaExist.UserId,
                        Name = ventaExist.Name,
                        LastName = ventaExist.LastName,
                        Email = ventaExist.Email,
                        Phone = ventaExist.Phone,
                        Estado = "Reembolsado",
                        Emision = DateTime.UtcNow,
                    };

                    _db.VentaTbl.Update(updateVenta);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha efectuado el reembolso con éxito";
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}