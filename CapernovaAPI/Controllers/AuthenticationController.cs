using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Authentication.Login;
using User.Managment.Data.Models.Authentication.SignUp;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager; //Permite controlar el envio del codigo OTP
        private readonly IEmailRepository _emailRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
            , IConfiguration configuration, IEmailRepository emailRepository, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailRepository = emailRepository;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role = "User")
        {
            //Se chequea si el usuario existe
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "El usuario ya esta registrado" });
            }
            //Se prepara el user con la informacion que se va almacenar en la base de datos
            ApplicationUser user = new()
            {
                Name = registerUser.Name,
                UserName = registerUser.Name,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                PasswordHash = registerUser.Password,
                PhoneNumber = registerUser.Phone,
                Cuidad = registerUser.City,
                SecurityStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = true
            };
            //Se consulta si el rol existe en la base de datos, esto para poder asignarle uno existente
            if (await _roleManager.RoleExistsAsync(role))
            {
                //Se almacena en la base de datos
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    List<string> errores = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errores.Add(error.Description);
                    }
                    
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message =  errores.First() });

                }

                //Se le asigna el rol al usuario 
                await _userManager.AddToRoleAsync(user, role);

                //Se agrega el token para verificar el email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email }, "Enlace de confirmación de correo", $"Para confirmar presiona el <a href='{confirmationLink!}'>enlace</a>");
                _emailRepository.SendEmail(message);

                return StatusCode(StatusCodes.Status201Created,
                        new Response { Status = "Success", Message = "El usuario ha sido registrado y se ha enviado un correo para su confirmación" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "El rol no existe!" });
            }
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "Correo verificado correctamente" });
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "El usuario no ha confirmado su correo" });

        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {            
            //Verificar la existencia del usario
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            //Verificar la respuesta del user 
            if (user != null )
            {
                //Se valida la contraseña de usuario
                if (await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    //Se averigua si el usuario ha confirmado su correo electrónico
                    if (await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ////Se generan claims(informacion de usuario)
                        //var authClaims = new List<Claim>
                        //{
                        //    new Claim(ClaimTypes.Name, user.UserName),
                        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        //};
                        ////Se agrega el rol de usuario a la lista
                        //var userRoles = await _userManager.GetRolesAsync(user);
                        //foreach (var role in userRoles)
                        //{
                        //    authClaims.Add(new Claim(ClaimTypes.Role, role));
                        //}

                        // Se consulta si se tiene activado el TwoFactor
                        if (user.TwoFactorEnabled)
                        {
                            //Permite desconectar a los usuarios que esten logeados
                            await _signInManager.SignOutAsync();
                            //permite logear a un usuario de manera aisncronica
                            await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, true);

                            //Se genera el codigo para le ingreso a la plataforma
                            var token = await _userManager.GenerateTwoFactorTokenAsync(user,"Email");
                        
                            //Se envia el mensaje al correo 
                            var message = new Message(new string[] { user.Email! }, "Código para ingreso a la plataforma", $"El codigo es {token!}");
                            _emailRepository.SendEmail(message);

                            return StatusCode(StatusCodes.Status200OK,
                                new Response { Status = "Success", Message = $"Se ha enviado un codigo al correo {user.Email}" });
                        }

                        ////Se genera el token con los claims                  
                        //var jwtToken = GetToken(authClaims);
                        ////se regresa un token de acceso

                        //return Ok(new
                        //{
                        //    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        //    expiration = jwtToken.ValidTo
                        //});


                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Error", Message = "El usuario no ha verificado su cuenta" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Error", Message = "El usuario o la contraseña no es válido. Intentelo nuevamente!" });
                } 
            }

            return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Error", Message = "Usuario no registrado!" });
        }




        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            //Se almacena el resultado de haber ingresado el codigo generado OTP
            var signIn = await _signInManager.TwoFactorSignInAsync("Email", code, false, false);
            //Si el codigo se ingreso correctamente se obtendra un resultado succeeded= true
            if (signIn.Succeeded)
            {
                if (user != null )
                {
                    //Se generan claims(informacion de usuario)
                    var authClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.UserName),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                            };
                    //Se agrega el rol de usuario a la lista
                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    //Se genera el token con los claims                  
                    var jwtToken = GetToken(authClaims);
                    //se regresa un token de acceso

                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    });
                    
                }

            }
            return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Error", Message = "El codigo es incorrecto" });
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-Password")]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            //Se busca al usuario
            var user = await _userManager.FindByEmailAsync(email);
            //Se verifica el resultado de la busqueda de usuario
            if ( user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email }, "Solicitud de cambio de contraseña", $"Para cambiar tu conraseña presiona el <a href='{forgotPasswordLink!}'>enlace</a>");
                _emailRepository.SendEmail(message);

                return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = $"Solicitud de cambio de contraseña ha sido enviada al correo {user.Email}. Por favor, revise su correo" });
            }
            return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Error", Message = "El usuario no existe. No se pudo realizar la solicitud de cambio de contraseña" });
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model =  new ResetPassword { Token = token, Email = email };
            return Ok(new
            {
                model

            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            //Se busca el usuario 
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user!=null)
            {
                var resetResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!resetResult.Succeeded)
                {
                    foreach (var error in resetResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }
                return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = "Su contraseña ha sido cambiada" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Error", Message = "No se pudo actualizar su contraseña." });

        }




        private JwtSecurityToken GetToken(List<Claim> authClaim)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

    }
}
