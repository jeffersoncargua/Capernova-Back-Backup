// <copyright file="PedidoDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace User.Managment.Data.Models.Ventas.Dto
{
    public class PedidoDto
    {
        public int Id { get; set; }

        public DateTime Emision { get; set; }

        public string? Productos { get; set; }

        public int VentaId { get; set; }

        public string? DirectionMain { get; set; }

        public string? DirectionSec { get; set; }

        public string? Estado { get; set; }
    }
}