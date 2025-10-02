// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Linq.Expressions;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IRepository<T>
        where T : class
    {
        // el Tracked permite que se evaluen los items para po tener el conflicto cuando existen multiples ID y no se puede editar o eliminar posteriormente
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null);

        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null);

        Task CreateAsync(T entity);

        Task RemoveAsync(T entity);

        Task SaveAsync();
    }
}