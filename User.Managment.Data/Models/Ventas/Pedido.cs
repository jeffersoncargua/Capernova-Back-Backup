// <copyright file="Pedido.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Managment.Data.Models.Ventas
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Emision { get; set; }

        public string? Productos { get; set; }

        [Required]
        public int VentaId { get; set; }

        [ForeignKey("VentaId")]
        public Venta? Venta { get; set; }

        public string? DirectionMain { get; set; }

        public string? DirectionSec { get; set; }

        public string? Estado { get; set; }
    }
}