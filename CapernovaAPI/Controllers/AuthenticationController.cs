// <copyright file="AuthenticationController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Managment.Data.Models.Authentication.Login;
using User.Managment.Data.Models.Authentication.SignUp;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _auth;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public AuthenticationController(IAuthenticationRepository auth, IMapper mapper)
        {
            this._response = new();
            _auth = auth;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterUserDto registerUser)
        {
            var result = await _auth.Register(registerUser);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var result = await _auth.ConfirmEmail(token, email);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [Route("login")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _auth.Login(loginDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [Route("forgot-Password")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            var result = await _auth.ForgotPassword(forgotPassword);
            _response.StatusCode = result.StatusCode;
            _response.isSuccess = result.IsSuccess;
            _response.Message = result.Message;

            return StatusCode((int)result.StatusCode, _response);
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token, string email)
        {
            var result = _auth.ResetPasword(token, email);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            var result = await _auth.ResetPasword(resetPassword);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}