// <copyright file="ConfirmOrder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace User.Managment.Data.Models.PaypalOrder
{
    public class ConfirmOrder
    {
        public string? IdentifierName { get; set; }

        public string? Productos { get; set; }

        public string? Total { get; set; }

        public string? Orden { get; set; }

        public string? TransaccionId { get; set; }
    }
}