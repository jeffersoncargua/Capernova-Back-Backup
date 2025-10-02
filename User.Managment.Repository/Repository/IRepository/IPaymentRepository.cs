// <copyright file="IPaymentRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.PaypalOrder;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IPaymentRepository
    {
        Task<ResponseDto> PaypalCardAsync(Order model);

        Task<ResponseDto> ConfirmPaypalAsync(string token);

        Task<ResponseDto> CreateOrderAsync(ConfirmOrder confirmOrder);
    }
}