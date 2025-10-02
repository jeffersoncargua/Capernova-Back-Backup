// <copyright file="IPruebaRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IPruebaRepository : IRepository<Prueba>
    {
        Task<ResponseDto> GetAllTest(int? id);

        Task<ResponseDto> GetTest(int? id);

        Task<ResponseDto> CreateTest(PruebaDto pruebaDto);

        Task<ResponseDto> UpdateTest(int id, PruebaDto pruebaDto);

        Task<ResponseDto> DeleteTest(int id);
    }
}