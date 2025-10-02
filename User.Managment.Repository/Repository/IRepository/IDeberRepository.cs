// <copyright file="IDeberRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IDeberRepository : IRepository<Deber>
    {
        Task<ResponseDto> UpdateDeberAsync(int id, DeberDto deber);

        Task<ResponseDto> CreateDeberAsync(DeberDto deber);

        Task<ResponseDto> DeleteDeberAsync(int id);

        Task<ResponseDto> GetAllDeberesAsync(int? id);

        Task<ResponseDto> GetDeberAsync(int id);

        // Task<List<Deber>> UpdateRangesAsync(List<Deber> entities);
    }
}