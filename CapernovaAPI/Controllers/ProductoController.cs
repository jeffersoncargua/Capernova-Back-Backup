using Microsoft.AspNetCore.Mvc;
using System.Net;
using User.Managment.Data.Models;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Data.Models.ProductosServicios.Dto;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepositoy _dbProducto;
        protected ApiResponse _response;
        public ProductoController(IProductoRepositoy dbProducto)
        {
            _dbProducto = dbProducto;
            this._response = new();
        }

        /// <summary>
        /// Este controlador permite obtener todos los productos registrados hasta el momento en la base de datos
        /// </summary>
        /// <param name="search">Es el modelo que permite obtener producto en especifico de acuerdo al titulo del producto</param>
        /// <returns>Retorna una lista de cursos y en el caso de tener una busqueda especifica retorna un listado con los cursos especificos</returns>
        [HttpGet]
        [Route("getAllProducto")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string? search, [FromQuery] string? tipo)
        {
            try
            {
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(tipo))
                {
                    var productoQuery = await _dbProducto.GetAllAsync(u => u.Titulo.ToLower().Contains(search) && u.Tipo == tipo);
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = productoQuery;
                    return Ok(_response);
                }
                else if(!string.IsNullOrEmpty(tipo))
                {
                    var productoQuery = await _dbProducto.GetAllAsync(u => u.Tipo == tipo);
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = productoQuery;
                    return Ok(_response);
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    var productoQuery = await _dbProducto.GetAllAsync(u => u.Titulo.ToLower().Contains(search));
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = productoQuery;
                    return Ok(_response);
                }

                var productos = await _dbProducto.GetAllAsync(); //devuelve una lista con los productos 
                                                               //var productos = await _db.ProducotsTbl.ToListAsync(); //si funciona la linea anterior se elimina esta linea

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la lista de productos";
                _response.Result = productos;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;

        }

        /// <summary>
        /// Este controlador permite obtener el curso de acuerdo al id del producto que se desea obtener
        /// </summary>
        /// <param name="id">Es el que contiene el identificador a comparar para obtener el producto</param>
        /// <returns></returns>
        [HttpGet("getProducto/{id:int}", Name = "getProducto")]
        public async Task<ActionResult<ApiResponse>> GetProducto(int id)
        {
            try
            {
                var producto = await _dbProducto.GetAsync(u => u.Id == id);// devuelve el producto cuyo id sea igual al Id del producto
                if (producto == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el producto con exito!";
                _response.Result = producto;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Route("createProducto")]
        public async Task<ActionResult<ApiResponse>> CreateProducto([FromBody] ProductoDto productoDto)
        {
            try
            {
                if (await _dbProducto.GetAsync(u => u.Titulo == productoDto.Titulo || u.Codigo == productoDto.Codigo) != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El producto ya esta registrado con un código o titulo similar";
                    return BadRequest(_response);
                }

                Producto model = new()
                {
                    Codigo = productoDto.Codigo,
                    ImagenUrl = productoDto.ImagenUrl,
                    Titulo = productoDto.Titulo,
                    Detalle = productoDto.Detalle,
                    Precio = productoDto.Precio,
                    Tipo = productoDto.Tipo!,
                    Cantidad = productoDto.Cantidad,

                };

                await _dbProducto.CreateAsync(model);
                await _dbProducto.SaveAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.Message = "El producto ha sido registrado con exito";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        [HttpPut("updateProducto/{id:int}", Name = "UpdateProducto")]
        public async Task<ActionResult<ApiResponse>> UpdateProducto(int id, [FromBody] ProductoDto productoDto)
        {
            try
            {
                var productoFromDb = await _dbProducto.GetAsync(u => u.Id == productoDto.Id, tracked: false);
                if (productoFromDb == null || productoDto == null || id != productoDto.Id)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe";
                    return BadRequest(_response);
                }

                Producto model = new()
                {
                    Id = productoDto.Id,
                    Codigo = productoDto.Codigo,
                    ImagenUrl = productoDto.ImagenUrl,
                    Titulo = productoDto.Titulo,
                    Detalle = productoDto.Detalle,
                    Precio = productoDto.Precio,
                    Tipo = productoDto.Tipo!,
                    Cantidad = productoDto.Cantidad,
                };

                await _dbProducto.UpdateAsync(model);
                await _dbProducto.SaveAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El producto ha sido actualizado con exito";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpDelete("deleteProducto/{id:int}", Name = "deleteProducto")]
        public async Task<ActionResult<ApiResponse>> DeleteProducto(int id)
        {
            try
            {
                var producto = await _dbProducto.GetAsync(u => u.Id == id);
                if (producto == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                    return BadRequest(_response);
                }

                await _dbProducto.RemoveAsync(producto);
                await _dbProducto.SaveAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El producto ha sido eliminado con exito";
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
