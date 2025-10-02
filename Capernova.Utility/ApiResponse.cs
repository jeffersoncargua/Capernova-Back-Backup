// <copyright file="ApiResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Net;

namespace Capernova.Utility
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Errors = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }

        public bool isSuccess { get; set; } = true;

        public object? Result { get; set; } = null;

        public string? Message { get; set; }

        public List<string> Errors { get; set; }
    }
}