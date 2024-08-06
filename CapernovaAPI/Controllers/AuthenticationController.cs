using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
        protected ApiResponse _response;
        private string secretKey;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
            , IConfiguration configuration, IEmailRepository emailRepository, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailRepository = emailRepository;
            _configuration = configuration;
            _signInManager = signInManager;
            this._response = new();
            secretKey = configuration.GetValue<string>("JWT:Secret");
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterUser registerUser)
        {
            try
            {
                //Se chequea si el usuario existe
                var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
                if (userExist != null)
                {
                    //return StatusCode(StatusCodes.Status403Forbidden,
                    //    new Response { Status = "Error", Message = "El usuario ya esta registrado" });
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El usuario ya esta registrado";
                    _response.isSuccess = false;
                    return BadRequest(_response);
                }
                //Se prepara el user con la informacion que se va almacenar en la base de datos
                ApplicationUser user = new()
                {
                    Name = registerUser.Name,
                    UserName = registerUser.Email,
                    LastName = registerUser.LastName,
                    Email = registerUser.Email,
                    PasswordHash = registerUser.Password,
                    PhoneNumber = registerUser.Phone,
                    Ciudad = registerUser.City,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    TwoFactorEnabled = true,
                };
                //Se consulta si el rol existe en la base de datos, esto para poder asignarle uno existente
                //string role = "User";
                if (await _roleManager.RoleExistsAsync(registerUser.Role))
                {
                    //Se almacena en la base de datos
                    var result = await _userManager.CreateAsync(user, registerUser.Password);
                    if (!result.Succeeded)
                    {
                        //List<string> errores = new List<string>();
                        foreach (var error in result.Errors)
                        {
                            _response.Errors.Add(error.Description);
                        }

                        //return StatusCode(StatusCodes.Status500InternalServerError,
                        //    new Response { Status = "Error", Message =  errores.First() });
                        _response.isSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = _response.Errors.FirstOrDefault()!;
                        return BadRequest(_response);

                    }

                    //Se le asigna el rol al usuario 
                    await _userManager.AddToRoleAsync(user, registerUser.Role);

                    //Se agrega el token para verificar el email
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var tokenEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //Se codifica el token para que en base 64 para que no coloca caracteres que luego se eliminen
                    //var tokenEncode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(tokenEmail));
                    //Se genera el link para enviar el token y el usuario
                    //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email }, Request.Scheme);
                    var confirmationLink = $"https://localhost:3000/confirmEmail?token={tokenEmail}&email={user.Email}";


                    var message = new Message(new string[] { user.Email }, "Enlace de confirmación de correo", $"Para confirmar presiona el <a href='{confirmationLink!}'>enlace</a>");
                    //var message = new Message(new string[] { user.Email }, "Enlace de confirmación de correo", $"Para confirmar presiona el enlace<a href='http://localhost:3000/confirmEmail'>{confirmationLink!}</a>");
                    _emailRepository.SendEmail(message);

                    //return StatusCode(StatusCodes.Status201Created,
                    //        new Response { Status = "Success", Message = "El usuario ha sido registrado y se ha enviado un correo para su confirmación" });
                    _response.StatusCode = HttpStatusCode.Created;
                    _response.isSuccess = true;
                    _response.Message = "El usuario ha sido registrado y se ha enviado un correo para su confirmación";
                    return Ok(_response);
                }
                else
                {
                    //return StatusCode(StatusCodes.Status500InternalServerError,
                    //    new Response { Status = "Error", Message = "El rol no existe!" });
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.isSuccess = false;
                    _response.Message = "El rol no existe";
                    return NotFound(_response);
                }


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };

            }

            return _response;

            
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            //Se debe reemplazar los espacion en blanco con un +
            //var decodeToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            token = token.Replace(" ", "+");
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    //return StatusCode(StatusCodes.Status200OK,
                    //new Response { Status = "Success", Message = "Correo verificado correctamente" });
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.isSuccess = true;
                    _response.Message = "Correo verificado correctamente";
                    return NotFound(_response);
                }
            }

            //return StatusCode(StatusCodes.Status500InternalServerError,
            //        new Response { Status = "Error", Message = "El usuario no ha confirmado su correo" });
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.isSuccess = false;
            _response.Message = "El usuario no ha confirmado su correo";
            return NotFound(_response);

        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginModel loginModel)
        {
            try 
            { 
                //Verificar la existencia del usario
                var user = await _userManager.FindByEmailAsync(loginModel.Email);
                //Verificar la respuesta del user 
                if (user != null)
                {
                    //Se valida la contraseña de usuario
                    if (await _userManager.CheckPasswordAsync(user, loginModel.Password))
                    {
                        //Se averigua si el usuario ha confirmado su correo electrónico
                        if (await _userManager.IsEmailConfirmedAsync(user))
                        {
                            //Se obtiene el rol de usuario
                            var userRoles = await _userManager.GetRolesAsync(user);

                            //Se genera un jwt Token
                            var tokenHandler = new JwtSecurityTokenHandler();
                            //Se obtiene la clave de JWT:Secret del archivo appSettings.json
                            var key = Encoding.ASCII.GetBytes(secretKey);

                            var tokenDescriptior = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                 new Claim(ClaimTypes.Name, user.UserName),
                                 new Claim(ClaimTypes.Role, userRoles.First()),
                                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                                 new Claim(ClaimTypes.Actor, user.Name),
                                 new Claim(ClaimTypes.GivenName, user.LastName),
                                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                                }),
                                Expires = DateTime.UtcNow.AddDays(7),
                                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                            };



                            //Se generan claims(informacion de usuario)
                            //var authClaims = new List<Claim>
                            //{
                            //    new Claim(ClaimTypes.Name, user.UserName),
                            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                            //};
                            //Se agrega el rol de usuario a la lista
                            //var userRoles = await _userManager.GetRolesAsync(user);
                            //foreach (var role in userRoles)
                            //{
                            //    authClaims.Add(new Claim(ClaimTypes.Role, role));
                            //}

                            // Se consulta si se tiene activado el TwoFactor
                            //if (user.TwoFactorEnabled)
                            //{
                            //    //Permite desconectar a los usuarios que esten logeados
                            //    await _signInManager.SignOutAsync();
                            //    //permite logear a un usuario de manera asincronica
                            //    await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, true);                            

                            //    //Se genera el codigo para le ingreso a la plataforma
                            //    var token = await _userManager.GenerateTwoFactorTokenAsync(user,"Email");



                            //    //Se envia el mensaje al correo 
                            //    var message = new Message(new string[] { user.Email! }, "Código para ingreso a la plataforma", $"El codigo es {token!}");
                            //    _emailRepository.SendEmail(message);

                            //    //return StatusCode(StatusCodes.Status200OK,
                            //    //    new Response { Status = "Success", Message = $"Se ha enviado un codigo al correo {user.Email}" });
                            //    _response.StatusCode = HttpStatusCode.OK;
                            //    _response.isSuccess = true;
                            //    _response.Message = $"Se ha enviado un codigo al correo {user.Email}";
                            //    return Ok(_response);
                            //}

                            //Se genera el token con los claims                  
                            //var jwtToken = GetToken(authClaims);
                            //se regresa un token de acceso

                            //return Ok(new
                            //{
                            //    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                            //    expiration = jwtToken.ValidTo
                            //});

                            var token = tokenHandler.CreateToken(tokenDescriptior);



                            _response.StatusCode = HttpStatusCode.OK;
                            _response.isSuccess = true;
                            _response.Message = "Login exitoso";
                            _response.Result = new
                            {
                                Token = tokenHandler.WriteToken(token),
                                expiration = token.ValidTo
                            };
                            return Ok(_response);

                        }
                        else
                        {
                            //return StatusCode(StatusCodes.Status400BadRequest,
                            //new Response { Status = "Error", Message = "El usuario no ha verificado su cuenta" });
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.isSuccess = false;
                            _response.Message = "El usuario no ha verificado su cuenta";
                            return BadRequest(_response);
                        }
                    }
                    else
                    {
                        //return StatusCode(StatusCodes.Status400BadRequest,
                        //    new Response { Status = "Error", Message = "El usuario o la contraseña no es válido. Intentelo nuevamente!" });
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.isSuccess = false;
                        _response.Message = "El usuario o la contraseña no es válido. Intentelo nuevamente!";
                        return BadRequest(_response);
                    }
                }

                //return StatusCode(StatusCodes.Status400BadRequest,
                //            new Response { Status = "Error", Message = "Usuario no registrado!" });
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.isSuccess = false;
                _response.Message = "Usuario no registrado!";
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
            
           
        }




        [HttpPost]
        [Route("login-2FA")]
        public async Task<ActionResult<ApiResponse>> LoginWithOTP([FromBody] LoginOTP loginOTP)
        {
            var user = await _userManager.FindByEmailAsync(loginOTP.Email);

            
            //Se almacena el resultado de haber ingresado el codigo generado OTP
            var signIn = await _signInManager.TwoFactorSignInAsync("Email", loginOTP.OTP, false, false);
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
                    //var jwtToken = GetToken(authClaims);
                    //se regresa un token de acceso

                    //return Ok(new
                    //{
                    //    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    //    expiration = jwtToken.ValidTo
                    //});

                    _response.StatusCode = HttpStatusCode.OK;
                    _response.isSuccess = true;
                    _response.Message = "Login exitoso";
                    _response.Result = new
                    {
                        //Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        //expiration = jwtToken.ValidTo
                    };
                    return Ok(_response);

                }

            }

            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.isSuccess = false;
            _response.Message = "El código ingresado es incorrecto";            
            return BadRequest(_response);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-Password")]
        public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            //Se busca al usuario
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            //Se verifica el resultado de la busqueda de usuario
            if ( user != null)
            {
                //Se obtiene el token para poder cambiar de contraseña
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var forgotPasswordLink = $"https://localhost:3000/changePassword?token={token}&email={user.Email}";

                var message = new Message(new string[] { user.Email }, "Solicitud de cambio de contraseña", $"Para cambiar tu contraseña presiona el <a href='{forgotPasswordLink!}'>enlace</a>");
                _emailRepository.SendEmail(message);

                //return StatusCode(StatusCodes.Status200OK,
                //        new Response { Status = "Success", Message = $"Solicitud de cambio de contraseña ha sido enviada al correo {user.Email}. Por favor, revise su correo" });
                _response.StatusCode = HttpStatusCode.OK;
                _response.isSuccess = true;
                _response.Message = $"La solicitud de cambio de contraseña ha sido enviada al correo {user.Email}. Por favor, revise su correo";
                return Ok(_response);
            }
            //return StatusCode(StatusCodes.Status404NotFound,
            //            new Response { Status = "Error", Message = "El usuario no existe. No se pudo realizar la solicitud de cambio de contraseña" });
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.isSuccess = false;
            _response.Message = "El usuario no existe. No se pudo realizar la solicitud de cambio de contraseña";
            return NotFound(_response);
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model =  new ResetPassword { Token = token, Email = email };
            //return Ok(new
            //{
            //    model

            //});
            _response.StatusCode = HttpStatusCode.OK;
            _response.isSuccess = true;
            _response.Message = "";
            _response.Result = new
            {
                model
            };
            return Ok(_response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            //Se agrega el simbolo + ya que en el fetch este simbolo se borra
            resetPassword.Token = resetPassword.Token.Replace(" ", "+");

            //Se busca el usuario 
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user!=null)
            {
                var resetResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!resetResult.Succeeded)
                {
                    foreach (var error in resetResult.Errors)
                    {
                        //ModelState.AddModelError(error.Code, error.Description);
                        _response.Errors.Add(error.Description);
                    }

                    //return Ok(ModelState);
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.isSuccess = false;
                    _response.Message = "No se pudo realizar el cambio de contraseña. Intentelo nuevamente";
                    return NotFound(_response);
                }
                //return StatusCode(StatusCodes.Status200OK,
                //        new Response { Status = "Success", Message = "Su contraseña ha sido cambiada" });
                _response.StatusCode = HttpStatusCode.OK;
                _response.isSuccess = true;
                _response.Message = "Su contraseña ha sido cambiada correctamente";
                return Ok(_response);
            }
            //return StatusCode(StatusCodes.Status400BadRequest,
            //            new Response { Status = "Error", Message = "No se pudo actualizar su contraseña." });
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.isSuccess = false;
            _response.Message = "No se pudo actualizar su contraseña.";
            return BadRequest(_response);

        }




        //private JwtSecurityToken GetToken(List<Claim> authClaim)
        //{
        //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["JWT:ValidIssuer"],
        //        audience: _configuration["JWT:ValidAudience"],
        //        expires: DateTime.Now.AddHours(3),
        //        claims: authClaim,
        //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //        );

        //    return token;
        //}

    }
}
