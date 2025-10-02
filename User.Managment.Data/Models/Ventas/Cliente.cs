// <copyright file="Cliente.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace User.Managment.Data.Models.Ventas
{
    public class Cliente
    {
        public string? Id { get; set; } // cedula

        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? DirectionMain { get; set; }

        public string? DirectionSec { get; set; }
    }
}