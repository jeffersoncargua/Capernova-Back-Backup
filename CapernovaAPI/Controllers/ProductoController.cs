using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using User.Managment.Data.Data;
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
        private readonly ApplicationDbContext _db;
        private readonly IProductoRepositoy _dbProducto;
        protected ApiResponse _response;
        public ProductoController(IProductoRepositoy dbProducto, ApplicationDbContext db)
        {
            _dbProducto = dbProducto;
            _db = db;
            this._response = new();
        }

        /// <summary>
        /// Este controlador permite obtener todos los productos registrados hasta el momento en la base de datos
        /// </summary>
        /// <param name="search">Es el modelo que permite obtener producto en especifico de acuerdo al titulo del producto</param>
        /// <returns>Retorna una lista de cursos y en el caso de tener una busqueda especifica retorna un listado con los cursos especificos</returns>
        [HttpGet]
        [Route("getAllProducto")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string? search, [FromQuery] string? tipo, [FromQuery] int? categoriaId = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(tipo) && categoriaId != 0)
                {
                    var productoQuery = await _dbProducto.GetAllAsync(u => u.Titulo!.ToLower().Contains(search) && u.Tipo == tipo && u.CategoriaId == categoriaId);
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = productoQuery;
                    return Ok(_response);
                }
                else if (!string.IsNullOrEmpty(tipo) && categoriaId != 0)
                {
                    var productoQuery = await _dbProducto.GetAllAsync(u => u.Tipo == tipo && u.CategoriaId == categoriaId);
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = productoQuery;
                    return Ok(_response);
                }
                else if (!string.IsNullOrEmpty(search) && categoriaId != 0)
                {
                    var productoQuery = await _dbProducto.GetAllAsync(u => u.Titulo!.ToLower().Contains(search) && u.CategoriaId == categoriaId);
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = productoQuery;
                    return Ok(_response);
                }
                else if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(tipo))
                {
                    var productoQuery = await _dbProducto.GetAllAsync(u => u.Titulo!.ToLower().Contains(search) && u.Tipo == tipo);
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = productoQuery;
                    return Ok(_response);
                }
                else if (!string.IsNullOrEmpty(tipo))
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
                    var productoQuery = await _dbProducto.GetAllAsync(u => u.Titulo!.ToLower().Contains(search));
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
                _response.Message = "Se ha obtenido el producto con éxito!";
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

        /// <summary>
        /// Este controlador permite obtener el curso de acuerdo al id del producto que se desea obtener
        /// </summary>
        /// <param name="id">Es el que contiene el identificador a comparar para obtener el producto</param>
        /// <returns></returns>
        [HttpGet("getProductoCode", Name = "getProductoCode")]
        public async Task<ActionResult<ApiResponse>> GetProductoCode([FromQuery]string codigo)
        {
            try
            {
                var producto = await _dbProducto.GetAsync(u => u.Codigo == codigo);// devuelve el producto cuyo id sea igual al Id del producto
                if (producto == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el producto con éxito!";
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
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateProducto([FromBody] ProductoDto productoDto)
        {
            try
            {
                if (productoDto.Cantidad < 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error al intentar agregar un producto con un stock menor a cero";
                    return BadRequest(_response);
                }

                if (productoDto.CategoriaId == 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Debe seleccionar una categoría para el producto";
                    return BadRequest(_response);
                }

                if (await _dbProducto.GetAsync(u => u.Titulo!.ToLower() == productoDto.Titulo!.ToLower() || u.Codigo == productoDto.Codigo) != null)
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
                    CategoriaId = productoDto.CategoriaId

                };

                await _dbProducto.CreateAsync(model);
                await _dbProducto.SaveAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.Message = "El producto ha sido registrado con éxito";
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
        [ResponseCache(CacheProfileName = "Default30")]
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

                if (productoDto.Cantidad < 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error al intentar agregar un producto con un stock menor a cero";
                    return BadRequest(_response);
                }

                if (productoDto.CategoriaId == 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Debe seleccionar una categoría para el producto";
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
                    CategoriaId = productoDto.CategoriaId
                };

                await _dbProducto.UpdateAsync(model);
                await _dbProducto.SaveAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El producto ha sido actualizado con éxito";
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
        [ResponseCache(CacheProfileName = "Default30")]
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
                _response.Message = "El producto ha sido eliminado con éxito";
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
        /// Este controlador permite obtener todos los productos registrados hasta el momento en la base de datos
        /// </summary>
        /// <param name="search">Es el modelo que permite obtener la categoria en especifico de acuerdo al nombre de la cateogria</param>
        /// <returns>Retorna una lista de categorias y en el caso de tener una busqueda especifica retorna un listado con los cursos especificos</returns>
        [HttpGet]
        [Route("getAllCategoria")]
        public async Task<ActionResult<ApiResponse>> GetAllCategoria([FromQuery] string? search, [FromQuery] string? tipo)
        {
            try
            {
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(tipo))
                {
                    var categotyQuery = await _db.CategoriaTbl.AsNoTracking().Where(u => u.Name!.ToLower().Contains(search) && u.Tipo == tipo).ToListAsync();
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de categorías";
                    _response.Result = categotyQuery;
                    return Ok(_response);
                }
                else if (!string.IsNullOrEmpty(tipo))
                {
                    var categotyQuery = await _db.CategoriaTbl.AsNoTracking().Where(u => u.Tipo == tipo).ToListAsync();
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de categorías";
                    _response.Result = categotyQuery;
                    return Ok(_response);
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    var categotyQuery = await _db.CategoriaTbl.AsNoTracking().Where(u => u.Name!.ToLower().Contains(search)).ToListAsync();
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de categorías";
                    _response.Result = categotyQuery;
                    return Ok(_response);
                }

                var categorias = await _db.CategoriaTbl.AsNoTracking().ToListAsync(); //devuelve una lista con los productos 
                                                                                                                                         //var productos = await _db.ProducotsTbl.ToListAsync(); //si funciona la linea anterior se elimina esta linea

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la lista de productos";
                _response.Result = categorias;
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
        [Route("createCategoria")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateCategoria([FromBody] CategoriaDto categoriaDto)
        {
            try
            {
                if (string.IsNullOrEmpty(categoriaDto.Tipo))
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Asegúrate de seleccionar el tipo de categoría.";
                    return BadRequest(_response);
                }

                if(categoriaDto == null || string.IsNullOrEmpty(categoriaDto.Name))
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Asegúrate de rellenar los campos.";
                    return BadRequest(_response);
                }

                var categoriaExist = await _db.CategoriaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Name!.ToLower() == categoriaDto.Name.ToLower() );
                if (categoriaExist != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro ya existe";
                    return BadRequest(_response);
                }

                Categoria model = new()
                {
                    Name = categoriaDto.Name,
                    Tipo = categoriaDto.Tipo
                };

                await _db.CategoriaTbl.AddAsync(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "La categoría ha sido registrada con éxito";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpPut]
        [Route("updateCategoria/{id:int}", Name = "updateCategoria")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateCategoria(int id, [FromBody] CategoriaDto categoriaDto)
        {
            try
            {
                if (string.IsNullOrEmpty(categoriaDto.Tipo))
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Asegúrate de seleccionar el tipo de categoría.";
                    return BadRequest(_response);
                }

                if (categoriaDto == null || string.IsNullOrEmpty(categoriaDto.Name))
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Asegúrate de rellenar los campos.";
                    return BadRequest(_response);
                }

                var categoriaExist = await _db.CategoriaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (categoriaExist != null && categoriaDto != null && categoriaDto.Id == id)
                {
                    Categoria model = new()
                    {
                        Id = categoriaDto.Id,
                        Name = categoriaDto.Name,
                        Tipo = categoriaDto.Tipo
                    };

                    _db.CategoriaTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "La categoría ha sido actualizado con éxito";
                    return Ok(_response);

                }

                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo actualizar la categoría";
                return BadRequest(_response);

            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        [HttpDelete("deleteCategoria/{id:int}", Name = "deleteCategoria")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteCategoria(int id)
        {
            try
            {
                var categoria = await _db.CategoriaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (categoria == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                    return BadRequest(_response);
                }

                _db.CategoriaTbl.Remove(categoria);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "La categoría ha sido eliminada con éxito";
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
