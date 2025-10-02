// <copyright file="Comentario.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Student
{
    public class Comentario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? PhotoUrl { get; set; }

        [Required]
        public string? FeedBack { get; set; }

        public string? Titulo { get; set; }
    }
}