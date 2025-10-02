// <copyright file="ShoppingCart.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Managment.Data.Models.Ventas
{
    [Index(nameof(ProductoCode))]
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? ProductoCode { get; set; }

        [Required]
        public string? ProductoName { get; set; }

        [Required]
        public string? ProductoImagen { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public double Total { get; set; }

        [Required]
        public int VentaId { get; set; }

        [ForeignKey("VentaId")]
        public Venta? Venta { get; set; }
    }
}