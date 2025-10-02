// <copyright file="Venta.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Ventas
{
    [Index(nameof(Emision))]
    [Index(nameof(UserId))]
    [Index(nameof(LastName))]
    [Index(nameof(Phone))]
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? TransaccionId { get; set; }

        public DateTime Emision { get; set; }

        public double Total { get; set; }

        public string? UserId { get; set; }

        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Estado { get; set; }
    }
}