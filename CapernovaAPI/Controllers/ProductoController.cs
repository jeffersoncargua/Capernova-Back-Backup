// <copyright file="ProductoController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using Microsoft.AspNetCore.Mvc;
using User.Managment.Data.Models.ProductosServicios.Dto;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository _repoProducto;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public ProductoController(IProductoRepository repoProducto, IMapper mapper)
        {
            _repoProducto = repoProducto;
            _mapper = mapper;
            this._response = new();
        }

        /// <summary>
        /// Este controlador permite obtener todos los productos registrados hasta el momento en la base de datos.
        /// </summary>
        /// <param name="search">Es el modelo que permite obtener producto en especifico de acuerdo al titulo del producto.</param>
        /// <param name="tipo">Es el tipo de producto que se solicita el usuario que puede ser curso o producto.</param>
        /// <param name="categoriaId">Es el identificado de la categoria para buscar la informacion realcionada al producto con la categoria.</param>
        /// <returns>Retorna una lista de cursos y en el caso de tener una busqueda especifica retorna un listado con los cursos especificos.</returns>
        [HttpGet]
        [Route("getAllProducto")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string? search, [FromQuery] string? tipo, [FromQuery] int? categoriaId = 0)
        {
            var result = await _repoProducto.GetAllProductsAsync(search, tipo, categoriaId);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        /// <summary>
        /// Este controlador permite obtener el curso de acuerdo al id del producto que se desea obtener.
        /// </summary>
        /// <param name="id">Es el que contiene el identificador a comparar para obtener el producto.</param>
        /// <returns>Retorna el producto que el usuario solicite.</returns>
        [HttpGet("getProducto/{id:int}", Name = "getProducto")]
        public async Task<ActionResult<ApiResponse>> GetProducto(int id)
        {
            var result = await _repoProducto.GetProductoAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        /// <summary>
        /// Este controlador permite obtener el curso de acuerdo al id del producto que se desea obtener.
        /// </summary>
        /// <param name="codigo">Es el codigo del producto.</param>
        /// <returns>Retorna el producto con el codigo especifico.</returns>
        [HttpGet("getProductoCode", Name = "getProductoCode")]
        public async Task<ActionResult<ApiResponse>> GetProductoCode([FromQuery] string codigo)
        {
            var result = await _repoProducto.GetProductoCodeAsync(codigo);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [Route("createProducto")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateProducto([FromBody] ProductoDto productoDto)
        {
            var result = await _repoProducto.CreateProductAsync(productoDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateProducto/{id:int}", Name = "UpdateProducto")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateProducto(int id, [FromBody] ProductoDto productoDto)
        {
            var result = await _repoProducto.UpdateProductAsync(id, productoDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete("deleteProducto/{id:int}", Name = "deleteProducto")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteProducto(int id)
        {
            var result = await _repoProducto.DeleteProductAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        /// <summary>
        /// Este controlador permite obtener todos los productos registrados hasta el momento en la base de datos.
        /// </summary>
        /// <param name="search">Es el modelo que permite obtener la categoria en especifico de acuerdo al nombre de la cateogria.</param>
        /// <param name="tipo">Es el tipo de categoria que se desea buscar que puede ser cremas, shampoo por ejemplo.</param>
        /// <returns>Retorna una lista de categorias y en el caso de tener una busqueda especifica retorna un listado con los cursos especificos.</returns>
        [HttpGet]
        [Route("getAllCategoria")]
        public async Task<ActionResult<ApiResponse>> GetAllCategoria([FromQuery] string? search, [FromQuery] string? tipo)
        {
            var result = await _repoProducto.GetAllCategoriaAsync(search, tipo);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [Route("createCategoria")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateCategoria([FromBody] CategoriaDto categoriaDto)
        {
            var result = await _repoProducto.CreateCategoryAsync(categoriaDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut]
        [Route("updateCategoria/{id:int}", Name = "updateCategoria")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateCategoria(int id, [FromBody] CategoriaDto categoriaDto)
        {
            var result = await _repoProducto.UpdateCategoryAsync(id, categoriaDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete("deleteCategoria/{id:int}", Name = "deleteCategoria")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteCategoria(int id)
        {
            var result = await _repoProducto.DeleteCategoryAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}