// <copyright file="FacturacionDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.PaypalOrder.Dto
{
    public class FacturacionDto
    {
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        public DateTime? FechaVenta { get; set; }

        public double Total { get; set; }
    }
}