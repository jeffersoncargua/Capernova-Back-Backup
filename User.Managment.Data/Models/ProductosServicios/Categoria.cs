// <copyright file="Categoria.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.ProductosServicios
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Tipo { get; set; }
    }
}