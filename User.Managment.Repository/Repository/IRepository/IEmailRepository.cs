// <copyright file="IEmailRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IEmailRepository
    {
        void SendEmail(Message message);
    }
}