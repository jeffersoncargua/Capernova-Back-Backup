// <copyright file="ShoppingCartDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.PaypalOrder.Dto
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int FacturaId { get; set; }

        [Required]
        public double Cantidad { get; set; }
    }
}