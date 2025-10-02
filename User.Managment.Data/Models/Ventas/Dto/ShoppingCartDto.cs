// <copyright file="ShoppingCartDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace User.Managment.Data.Models.Ventas.Dto
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }

        public string? Codigo { get; set; }

        public string? Titulo { get; set; } // Es el nombre del producto

        public string? Imagen { get; set; } // Es el nombre del producto

        public int Cantidad { get; set; }

        public double Precio { get; set; }

        public string? Tipo { get; set; } // Permite saber si el producto es de tipo curso o producto
    }
}