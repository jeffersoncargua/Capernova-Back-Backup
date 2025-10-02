// <copyright file="IMarketingRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IMarketingRepository : IRepository<Publicidad>
    {
        Task<ResponseDto> GetAllPublicidad(string? search);

        Task<ResponseDto> CreatePublicidad(PublicidadDto publicidadDto);

        Task<ResponseDto> DeletePublicidad(int id);

        Task<ResponseDto> UpdatetePublicidad(int id, PublicidadDto publicidadDto);
    }
}