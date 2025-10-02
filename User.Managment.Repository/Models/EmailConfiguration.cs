// <copyright file="EmailConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace User.Managment.Repository.Models
{
    public class EmailConfiguration
    {
        public string From { get; set; } = null!;

        public string SmtpServer { get; set; } = null!;

        public int Port { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}