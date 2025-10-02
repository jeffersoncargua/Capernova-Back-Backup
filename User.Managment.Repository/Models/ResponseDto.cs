// <copyright file="ResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Net;

namespace User.Managment.Repository.Models
{
    public class ResponseDto
    {
        public ResponseDto()
        {
            Errors = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        public object? Result { get; set; } = null;

        public string? Message { get; set; }

        public List<string> Errors { get; set; }
    }
}