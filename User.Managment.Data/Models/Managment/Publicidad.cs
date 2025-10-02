// <copyright file="Publicidad.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Managment.Data.Models.Managment
{
    public class Publicidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? imageUrl { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "Debe contener al menos 40 caracteres")]
        public string? Titulo { get; set; }
    }
}