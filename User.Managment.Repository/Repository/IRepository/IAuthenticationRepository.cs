// <copyright file="IAuthenticationRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.Authentication.Login;
using User.Managment.Data.Models.Authentication.SignUp;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IAuthenticationRepository
    {
        Task<ResponseDto> Register(RegisterUserDto registerUserDto);

        Task<ResponseDto> ConfirmEmail(string token, string email);

        Task<ResponseDto> Login(LoginDto loginDto);

        Task<ResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto);

        ResponseDto ResetPasword(string token, string email);

        Task<ResponseDto> ResetPasword(ResetPasswordDto resetPasswordDto);
    }
}