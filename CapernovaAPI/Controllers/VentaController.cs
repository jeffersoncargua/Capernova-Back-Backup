using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Data.Models.Ventas;
using User.Managment.Data.Models.Ventas.Dto;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IPedidoRepository _dbPedido;
        private readonly IMatriculaRepository _dbMatricula;
        protected ApiResponse _response;
        public VentaController(ApplicationDbContext db, IPedidoRepository dbPedido,IMatriculaRepository dbMatricula)
        {
            _db = db;
            _dbPedido = dbPedido;
            _dbMatricula = dbMatricula;
            this._response = new();
        }


        [HttpGet]
        [Route("getAllVentas")]
        public async Task<ActionResult<ApiResponse>> GetAllVentas([FromQuery] string? search, [FromQuery] string start, [FromQuery] string end)
        {
            try
            {
                
                if (!string.IsNullOrEmpty(search) && start != "null" && end != "null")
                {
                    var startDate = JsonConvert.DeserializeObject<DateTime>(start);
                    var endDate = JsonConvert.DeserializeObject<DateTime>(end);
                    var ventasQuery = await _db.VentaTbl.Where(u => (u.UserId.Contains(search) 
                    || u.LastName.ToLower().Contains(search) 
                    || u.Email.ToLower().Contains(search)
                    || u.TransaccionId.ToLower().Contains(search))
                    && u.Emision >= startDate  && u.Emision <= endDate).AsNoTracking().OrderByDescending(x => x.Emision).ToListAsync();
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de las ventas";
                    _response.Result = ventasQuery;
                    return Ok(_response);
                }
                else if (start != "null" && end != "null")
                {
                    var startDate = JsonConvert.DeserializeObject<DateTime>(start);
                    var endDate = JsonConvert.DeserializeObject<DateTime>(end);
                    var ventasQuery = await _db.VentaTbl.Where(u => u.Emision >= startDate && u.Emision <= endDate).AsNoTracking().OrderByDescending(x => x.Emision).ToListAsync();
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de las ventas";
                    _response.Result = ventasQuery;
                    return Ok(_response);
                }else if (!string.IsNullOrEmpty(search))
                {
                    var ventasQuery = await _db.VentaTbl.Where(u => u.UserId.Contains(search) 
                    || u.LastName.ToLower().Contains(search)
                    || u.Email.ToLower().Contains(search)
                    || u.TransaccionId.ToLower().Contains(search)).AsNoTracking().OrderByDescending(x => x.Emision).ToListAsync();
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de las ventas";
                    _response.Result = ventasQuery;
                    return Ok(_response);
                }
                

                var ventas = await _db.VentaTbl.AsNoTracking().OrderByDescending(x => x.Emision).ToListAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la lista de las ventas";
                _response.Result = ventas;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;

        }

        [HttpGet("getShoppingCart")]
        public async Task<ActionResult<ApiResponse>> GetShoppingCart([FromQuery] int ventaId )
        {
            try
            {
                var cartList = await _db.ShoppingCartTbl.Where(u => u.VentaId == ventaId).ToListAsync();
                if (cartList != null)
                {
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = $"Se ha obtenido la lista de los produtos pertenecientes a la venta {ventaId}";
                    _response.Result = cartList;
                    return Ok(_response);
                }

                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = $"Error. No se obtuvo la lista de los produtos pertenecientes a la venta {ventaId}";                
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;

        }

        [HttpDelete("deleteVenta/{id:int}", Name = "deleteVenta")]
        public async Task<ActionResult<ApiResponse>> DeleteVenta(int id)
        {
            try
            {
                var venta = await _db.VentaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (venta == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                    return BadRequest(_response);
                }

                _db.VentaTbl.Remove(venta);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "La venta ha sido eliminada con exito";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        [HttpGet]
        [Route("getAllPedidos")]
        public async Task<ActionResult<ApiResponse>> GetAllPedidos([FromQuery] string? search, [FromQuery] string start, [FromQuery] string end)
        {
            try
            {

                if (!string.IsNullOrEmpty(search) && start != "null" && end != "null")
                {
                    var startDate = JsonConvert.DeserializeObject<DateTime>(start);
                    var endDate = JsonConvert.DeserializeObject<DateTime>(end);
                    var pedidosQuery = await _dbPedido.GetAllAsync((u => (u.Venta!.LastName.Contains(search)
                    || u.Venta!.UserId.Contains(search)
                    || u.Venta.TransaccionId.ToLower().Contains(search)) 
                    && u.Emision >= startDate && u.Emision <= endDate),tracked:false,includeProperties:"Venta");
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de los pedidos";
                    _response.Result = pedidosQuery.OrderByDescending(x => x.Emision);
                    return Ok(_response);
                }
                else if (start != "null" && end != "null")
                {
                    var startDate = JsonConvert.DeserializeObject<DateTime>(start);
                    var endDate = JsonConvert.DeserializeObject<DateTime>(end);
                    var pedidosQuery = await _dbPedido.GetAllAsync((u => u.Emision >= startDate && u.Emision <= endDate), tracked: false, includeProperties: "Venta");
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de los pedidos";
                    _response.Result = pedidosQuery.OrderByDescending(x => x.Emision);
                    return Ok(_response);
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    var pedidosQuery = await _dbPedido.GetAllAsync((u => (u.Venta!.LastName.Contains(search)
                    || u.Venta!.UserId.Contains(search)
                    || u.Venta.TransaccionId.ToLower().Contains(search))), tracked: false, includeProperties: "Venta");
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de los pedidos";
                    _response.Result = pedidosQuery.OrderByDescending(x => x.Emision);
                    return Ok(_response);
                }


                var pedidos = await _dbPedido.GetAllAsync(tracked:false,includeProperties:"Venta");

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la lista de las pedidos";
                _response.Result = pedidos.OrderByDescending(x => x.Emision);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;

        }

        [HttpPut("updateVenta/{id:int}", Name = "updateVenta")]
        public async Task<ActionResult<ApiResponse>> UpdateVenta(int id)
        {
            try
            {
                var ventaExist = await _db.VentaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (ventaExist == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se pudo realizar efectuar el reembolso, inténtelo nuevamente";
                    return BadRequest(_response);
                }
                else
                {
                    var shoppingFromDb = await _db.ShoppingCartTbl.AsNoTracking().Where(u => u.VentaId == ventaExist.Id).ToListAsync();
                    if (shoppingFromDb == null)
                    {
                        _response.isSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "No se pudo realizar efectuar el reembolso, inténtelo nuevamente";
                        return BadRequest(_response);
                    }
                    else
                    {
                        foreach (var itemCart in shoppingFromDb)
                        {
                            var producto = await _db.ProductoTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Codigo == itemCart.ProductoCode);
                            if (producto == null)
                            {
                                _response.isSuccess = false;
                                _response.StatusCode = HttpStatusCode.BadRequest;
                                _response.Message = "No se pudo realizar efectuar el reembolso, inténtelo nuevamente";
                                return BadRequest(_response);
                            }
                            else if (producto.Tipo == "producto") // Esta funcion permite actualizar el stock de los productos del tipo producto cuando se realiza un reembolso
                            {
                                Producto productoModel = new()
                                {
                                    Id = producto.Id,
                                    Codigo = producto.Codigo,
                                    Titulo = producto.Titulo,
                                    Cantidad = producto.Cantidad + itemCart.Cantidad,
                                    ImagenUrl = producto.ImagenUrl,
                                    Precio = producto.Precio,
                                    Tipo = producto.Tipo,
                                    Detalle = producto.Detalle
                                };

                                _db.ProductoTbl.Update(productoModel);
                                await _db.SaveChangesAsync();
                            }
                            else if (producto.Tipo == "curso") //Esta funcion permite eliminar la matricula de un usuario cuando se ha realizado el reembolso
                            {
                                var matriculaExist = await _db.MatriculaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Curso.Codigo == itemCart.ProductoCode && u.EstudianteId == ventaExist.UserId);
                                if (matriculaExist != null)
                                {
                                    _db.MatriculaTbl.Remove(matriculaExist);
                                    await _db.SaveChangesAsync();
                                }
                            }

                            _db.ShoppingCartTbl.Remove(itemCart);
                            await _db.SaveChangesAsync();
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
                        Emision = DateTime.Now
                    };

                    _db.VentaTbl.Update(updateVenta);
                    await _db.SaveChangesAsync();

                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha efectuado el reembolso con exito";
                    return Ok(_response);
                }

            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        [HttpPut("updatePedido/{id:int}", Name = "updatePedido")]
        public async Task<ActionResult<ApiResponse>> UpdatePedido(int id, [FromBody] PedidoDto pedidoDto)
        {
            try
            {
                var pedidoFromDb = await _dbPedido.GetAsync(u => u.Id == pedidoDto.Id, tracked: false);
                if (pedidoFromDb == null || pedidoDto == null || id != pedidoDto.Id)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe";
                    return BadRequest(_response);
                }

                Pedido model = new()
                {
                    Id = pedidoFromDb.Id,
                    Emision = pedidoFromDb.Emision,
                    Productos = pedidoFromDb.Productos,
                    VentaId = pedidoFromDb.VentaId,
                    DirectionMain = pedidoDto.DirectionMain,
                    DirectionSec = pedidoDto.DirectionSec,
                    Estado = pedidoDto.Estado
                };

                await _dbPedido.UpdateAsync(model);
                await _dbPedido.SaveAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El pedido ha sido actualizado con exito";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpDelete("deletePedido/{id:int}", Name = "deletePedido")]
        public async Task<ActionResult<ApiResponse>> DeletePedido(int id)
        {
            try
            {
                var pedido = await _dbPedido.GetAsync(u => u.Id == id);
                if (pedido == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                    return BadRequest(_response);
                }

                await _dbPedido.RemoveAsync(pedido);
                await _dbPedido.SaveAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El pedido ha sido eliminado con exito";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;

        }
    }
}
