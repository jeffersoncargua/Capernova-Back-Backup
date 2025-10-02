// <copyright file="Producto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Managment.Data.Models.ProductosServicios
{
    [Index(nameof(Codigo), IsUnique = true)]
    [Index(nameof(Tipo))]
    [Index(nameof(CategoriaId))]
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Codigo { get; set; }

        [Required]
        public string? Titulo { get; set; }

        [Required]
        public int Cantidad { get; set; }

        public int? CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }

        public string? ImagenUrl { get; set; }

        public string? Detalle { get; set; }

        [Required]
        public double Precio { get; set; }

        public string? Tipo { get; set; }
    }
}