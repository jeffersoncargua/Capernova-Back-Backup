// <copyright file="ProductoRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Data.Models.ProductosServicios.Dto;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public ProductoRepository(ApplicationDbContext db, IMapper mapper)
            : base(db)
        {
            _db = db;
            _mapper = mapper;
            this._response = new();
        }

        public async Task<ResponseDto> CreateCategoryAsync(CategoriaDto categoriaDto)
        {
            try
            {
                if (string.IsNullOrEmpty(categoriaDto.Tipo))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Asegúrate de seleccionar el tipo de categoría.";
                }
                else
                {
                    if (categoriaDto == null || string.IsNullOrEmpty(categoriaDto.Name))
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "Asegúrate de rellenar los campos.";
                    }
                    else
                    {
                        var categoriaExist = await _db.CategoriaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Name!.ToLower() == categoriaDto.Name.ToLower());
                        if (categoriaExist != null)
                        {
                            _response.IsSuccess = false;
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.Message = "El registro ya existe";
                        }
                        else
                        {
                            await _db.CategoriaTbl.AddAsync(_mapper.Map<Categoria>(categoriaDto));
                            await _db.SaveChangesAsync();

                            _response.IsSuccess = true;
                            _response.StatusCode = HttpStatusCode.OK;
                            _response.Message = "La categoría ha sido registrada con éxito";
                        }
                    }
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

        public async Task<ResponseDto> CreateProductAsync(ProductoDto productoDto)
        {
            try
            {
                if (productoDto.Cantidad < 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error al intentar agregar un producto con un stock menor a cero";
                }
                else
                {
                    if (productoDto.CategoriaId == 0)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "Debe seleccionar una categoría para el producto";
                    }
                    else
                    {
                        if (await this.GetAsync(u => u.Titulo!.ToLower() == productoDto.Titulo!.ToLower() || u.Codigo == productoDto.Codigo, tracked: false) != null)
                        {
                            _response.IsSuccess = false;
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.Message = "El producto ya está registrado con un código o titulo similar";
                        }
                        else
                        {
                            await this.CreateAsync(_mapper.Map<Producto>(productoDto));

                            _response.IsSuccess = true;
                            _response.StatusCode = HttpStatusCode.Created;
                            _response.Message = "El producto ha sido registrado con éxito";
                        }
                    }
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

        public async Task<ResponseDto> DeleteCategoryAsync(int id)
        {
            try
            {
                var categoria = await _db.CategoriaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (categoria == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                    return _response;
                }

                var producto = await this.GetAsync(u => u.CategoriaId == id, tracked: false);
                if (producto != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error. No se puede eliminar una categoría que ya está asignada!!";
                    return _response;
                }

                _db.CategoriaTbl.Remove(categoria);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "La categoría ha sido eliminada con éxito";
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

        public async Task<ResponseDto> DeleteProductAsync(int id)
        {
            try
            {
                var producto = await this.GetAsync(u => u.Id == id, tracked: false);
                if (producto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                }
                else
                {
                    await this.RemoveAsync(producto);

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "El producto ha sido eliminado con éxito";
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

        public async Task<ResponseDto> GetAllCategoriaAsync(string? search, string? tipo)
        {
            try
            {
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(tipo))
                {
                    var categotyQuery = await _db.CategoriaTbl.AsNoTracking().Where(u => u.Name!.ToLower().Contains(search) && u.Tipo == tipo).ToListAsync();
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de categorías";
                    _response.Result = _mapper.Map<List<CategoriaDto>>(categotyQuery);
                }
                else if (!string.IsNullOrEmpty(tipo))
                {
                    var categotyQuery = await _db.CategoriaTbl.AsNoTracking().Where(u => u.Tipo == tipo).ToListAsync();
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de categorías";
                    _response.Result = _mapper.Map<List<CategoriaDto>>(categotyQuery);
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    var categotyQuery = await _db.CategoriaTbl.AsNoTracking().Where(u => u.Name!.ToLower().Contains(search)).ToListAsync();
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de categorías";
                    _response.Result = _mapper.Map<List<CategoriaDto>>(categotyQuery);
                }
                else
                {
                    var categorias = await _db.CategoriaTbl.AsNoTracking().ToListAsync(); // devuelve una lista con los productos

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de productos";
                    _response.Result = _mapper.Map<List<CategoriaDto>>(categorias);
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

        public async Task<ResponseDto> GetAllProductsAsync(string? search, string? tipo, int? categoriaId)
        {
            try
            {
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(tipo) && categoriaId != 0)
                {
                    var productoQuery = await this.GetAllAsync(u => u.Titulo!.ToLower().Contains(search) && u.Tipo == tipo && u.CategoriaId == categoriaId, tracked: false);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = _mapper.Map<List<ProductoDto>>(productoQuery);
                }
                else if (!string.IsNullOrEmpty(tipo) && categoriaId != 0)
                {
                    var productoQuery = await this.GetAllAsync(u => u.Tipo == tipo && u.CategoriaId == categoriaId, tracked: false);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = _mapper.Map<List<ProductoDto>>(productoQuery);
                }
                else if (!string.IsNullOrEmpty(search) && categoriaId != 0)
                {
                    var productoQuery = await this.GetAllAsync(u => u.Titulo!.ToLower().Contains(search) && u.CategoriaId == categoriaId, tracked: false);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = _mapper.Map<List<ProductoDto>>(productoQuery);
                }
                else if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(tipo))
                {
                    var productoQuery = await this.GetAllAsync(u => u.Titulo!.ToLower().Contains(search) && u.Tipo == tipo, tracked: false);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = _mapper.Map<List<ProductoDto>>(productoQuery);
                }
                else if (!string.IsNullOrEmpty(tipo))
                {
                    var productoQuery = await this.GetAllAsync(u => u.Tipo == tipo, tracked: false);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = _mapper.Map<List<ProductoDto>>(productoQuery);
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    var productoQuery = await this.GetAllAsync(u => u.Titulo!.ToLower().Contains(search), tracked: false);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Productos";
                    _response.Result = _mapper.Map<List<ProductoDto>>(productoQuery);
                }
                else
                {
                    var productos = await this.GetAllAsync(tracked: false); // devuelve una lista con los productos

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de productos";
                    _response.Result = _mapper.Map<List<ProductoDto>>(productos);
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

        public async Task<ResponseDto> GetProductoAsync(int id)
        {
            try
            {
                var producto = await this.GetAsync(u => u.Id == id, tracked: false); // devuelve el producto cuyo id sea igual al Id del producto
                if (producto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el producto con éxito!";
                    _response.Result = _mapper.Map<ProductoDto>(producto);
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

        public async Task<ResponseDto> GetProductoCodeAsync(string codigo)
        {
            try
            {
                var producto = await this.GetAsync(u => u.Codigo == codigo, tracked: false); // devuelve el producto cuyo id sea igual al Id del producto
                if (producto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el producto con éxito!";
                    _response.Result = _mapper.Map<ProductoDto>(producto);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateCategoryAsync(int id, CategoriaDto categoriaDto)
        {
            try
            {
                if (string.IsNullOrEmpty(categoriaDto.Tipo))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Asegúrate de seleccionar el tipo de categoría.";
                }
                else
                {
                    if (categoriaDto == null || string.IsNullOrEmpty(categoriaDto.Name))
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "Asegúrate de rellenar los campos.";
                    }
                    else
                    {
                        var categoriaExist = await _db.CategoriaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                        if (categoriaExist != null && categoriaDto != null && categoriaDto.Id == id)
                        {
                            _db.CategoriaTbl.Update(_mapper.Map<Categoria>(categoriaDto));
                            await _db.SaveChangesAsync();

                            _response.IsSuccess = true;
                            _response.StatusCode = HttpStatusCode.OK;
                            _response.Message = "La categoría ha sido actualizado con éxito";
                        }
                        else
                        {
                            _response.IsSuccess = false;
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.Message = "No se pudo actualizar la categoría";
                        }
                    }
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

        public async Task<ResponseDto> UpdateProductAsync(int id, ProductoDto productoDto)
        {
            try
            {
                var productoFromDb = await this.GetAsync(u => u.Id == productoDto.Id, tracked: false);
                if (productoFromDb == null || productoDto == null || id != productoDto.Id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe";
                    return _response;
                }

                if (productoFromDb.Codigo!.ToLower() != productoDto.Codigo!.ToLower() || productoFromDb.Titulo!.ToLower() != productoDto.Titulo!.ToLower())
                {
                    var productosCode = await this.GetAllAsync(u => u.Titulo!.ToLower() == productoDto.Titulo!.ToLower() || u.Codigo!.ToLower() == productoDto.Codigo!.ToLower(), tracked: false);
                    foreach (var item in productosCode)
                    {
                        if (item.Id != id)
                        {
                            _response.IsSuccess = false;
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.Message = "Error. No se puede actualizar el producto con un código o titulo que ya existe!";
                            return _response;
                        }
                    }
                }

                if (productoDto.Cantidad < 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error al intentar agregar un producto con un stock menor a cero";
                    return _response;
                }

                if (productoDto.CategoriaId == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Debe seleccionar una categoría para el producto";
                    return _response;
                }

                _db.ProductoTbl.Update(_mapper.Map<Producto>(productoDto));
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El producto ha sido actualizado con éxito";
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
    }
}