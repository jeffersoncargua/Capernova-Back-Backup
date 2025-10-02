// <copyright file="IMatriculaRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.PaypalOrder;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IMatriculaRepository : IRepository<Matricula>
    {
        Task<Matricula> UpdateAsync(Matricula entity);

        Task<List<Matricula>> UpdateRangesAsync(List<Matricula> entities);
    }
}