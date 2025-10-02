// <copyright file="ICapituloRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface ICapituloRepository : IRepository<Capitulo>
    {
        Task<ResponseDto> GetAllCapitulos(int? id);

        Task<ResponseDto> GetCapitulo(int id);

        Task<ResponseDto> CreateCapitulo(CapituloDto capituloDto);

        Task<ResponseDto> UpdateCapitulo(int id, CapituloDto capituloDto);

        Task<ResponseDto> DeleteCapitulo(int id);
    }
}