// <copyright file="IProductoRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Data.Models.ProductosServicios.Dto;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<ResponseDto> GetAllProductsAsync(string? search, string? tipo, int? categoriaId);

        Task<ResponseDto> GetAllCategoriaAsync(string? search, string? tipo);

        Task<ResponseDto> GetProductoAsync(int id);

        Task<ResponseDto> GetProductoCodeAsync(string codigo);

        Task<ResponseDto> CreateProductAsync(ProductoDto productoDto);

        Task<ResponseDto> CreateCategoryAsync(CategoriaDto categoriaDto);

        Task<ResponseDto> UpdateProductAsync(int id, ProductoDto productoDto);

        Task<ResponseDto> UpdateCategoryAsync(int id, CategoriaDto categoriaDto);

        Task<ResponseDto> DeleteProductAsync(int id);

        Task<ResponseDto> DeleteCategoryAsync(int id);
    }
}