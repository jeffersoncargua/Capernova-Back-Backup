using Capernova.Utility;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Authentication.SignUp;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Data.Models.PaypalOrder;
//using User.Managment.Data.Models.Managment;
//using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ManagmentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager; //Permite controlar el envio del codigo OTP
        private readonly IEmailRepository _emailRepository;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly ICourseRepositoty _dbCourse;
        private readonly FrontSettings _frontURL;
        protected ApiResponse _response;
        private string secretKey;

        public ManagmentController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
            , IConfiguration configuration, IEmailRepository emailRepository, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db, ICourseRepositoty dbCourse,
            FrontSettings frontURL)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailRepository = emailRepository;
            _configuration = configuration;
            _signInManager = signInManager;
            _db = db;
            _frontURL = frontURL;
            _dbCourse = dbCourse;
            this._response = new();
            secretKey = configuration.GetValue<string>("JWT:Secret");
        }

        /// <summary>
        /// Este controlador permite registrar usuarios con los tipos de roles existentes en la base de datos que son: User,Admin,Student,Teacher y Secretary
        /// </summary>
        /// <param name="registerUser">Es el modelo con la informacion que se recibe del front-end</param>
        /// <returns>Retorna la respuesta del registro que puede ser satisfactoria o no</returns>
        [HttpPost]
        [Route("registration")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterUser registerUser)
        {
            try
            {
                string textMessage = "";
                //Se chequea si el usuario existe
                var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
                if (userExist != null)
                {
                   
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El usuario ya está registrado";
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
                    var tokenEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //Se genera el link para enviar el token y el usuario
                    //var confirmationLink = $"https://localhost:3000/confirmEmail?token={tokenEmail}&email={user.Email}";
                    var confirmationLink = $"{_frontURL.Url}/confirmEmail?token={tokenEmail}&email={user.Email}";

                    //Se arma el mensaje que se le va a enviar el talento humano de Capernova
                    //textMessage += $"";
                    textMessage += $"<html>";
                    textMessage += $"<body>";
                    textMessage += $"<div style='display:flex; justify-content:center;'>";
                    textMessage += $"<img src=\"https://drive.google.com/thumbnail?id=1Io3SAYU468d_ekK2k7_Ic7u6UXoXj9eV\" alt=\"Aqui va una imagen\" />";
                    textMessage += $"</div>";
                    textMessage += $"<h1 style='text-align:center'><b>Bienvenido a Capernova!!</b></h1>";
                    textMessage += $"<h4><b>Nombre:</b> <span style='font-weight: normal;'>{registerUser.Name}</span></h4>";
                    textMessage += $"<h4><b>Apellido:</b> <span style='font-weight: normal;'>{registerUser.LastName}</span></h4>";
                    textMessage += $"<h4><b>Teléfono:</b> <span style='font-weight: normal;'>{registerUser.Phone}</span></h4>";
                    textMessage += $"<h4><b>Tus credenciales son:</b></h4>";
                    textMessage += $"<h4><b>Correo:</b> <span style='font-weight: normal;'>{registerUser.Email}</span></h4>";
                    textMessage += $"<h4><b>Password:</b> <span style='font-weight: normal;'>{registerUser.Password}</span></h4>";
                    textMessage += $"<br />";
                    textMessage += $"<div>";
                    textMessage += $"Para confirmar presiona el <a href='{confirmationLink!}'>enlace</a>";
                    textMessage += $"</div>";
                    textMessage += $"<p>Para mayor información comunicate al 0987203469, o envíanos un mensaje por nuestro whatsapp.</p>";
                    textMessage += $"<br />";
                    textMessage += $"<p>Ten un excelente día</p>";
                    textMessage += $"<br />";
                    textMessage += $"<p>Atentamente, Administración de Capernova</p>";
                    textMessage += $"</body>";
                    textMessage += $"</html>";



                    //Se arma el mensaje con el email del usuario registrado y el enlace de confirmacion del correo
                    var message = new Message(new string[] { user.Email! }, "Enlace de confirmación del talento humuno de capernova ", textMessage);
                    //Se envia el mensaje que se va a remitir por correo electrónico
                    _emailRepository.SendEmail(message);

                    if (registerUser.Role == "Teacher")
                    {
                        Teacher teacher = new()
                        {
                            Id = user.Id,
                            Name = user.Name,
                            LastName = user.LastName,
                            Email = user.Email,
                            Phone = user.PhoneNumber
                        };

                        await _db.TeacherTbl.AddAsync(teacher);
                        await _db.SaveChangesAsync();
                    }

                    
                    
                    _response.StatusCode = HttpStatusCode.Created;
                    _response.isSuccess = true;
                    _response.Message = "El usuario ha sido registrado y se ha enviado un correo para su confirmación";
                    return Ok(_response);
                }
                else
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.isSuccess = false;
                    _response.Message = "Debe seleccionar un rol de usuario!!";
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };

            }

            return _response;


        }

        /// <summary>
        /// Este controlador permite obtener todos los usuarios que se encuentren registrados en la base de datos, donde se realiza operaciones
        /// de Linq para poder obtener la union de las tablas de Users, UserRoles y Roles 
        /// </summary>
        /// <param name="searchRole">Permite buscar un rol en particular dentro de los usuarios con rol registrados</param>
        /// <param name="searchName">Permite buscar un usuario en particular en base a su nombre y apellido</param>
        /// <returns>Retorna una lista con los usurios registrados en la base de datos</returns>
        [HttpGet("getTalent")]
        public async Task<ActionResult<ApiResponse>> GetTalent([FromQuery] string? searchRole, [FromQuery] string? searchName)
        {
            try
            {

                var roles = await _db.Roles.ToListAsync();
                var rolAdmin = roles.Find(u => u.Name == "Admin");
                var rolSecretary = roles.Find(u => u.Name == "Secretary");
                var rolTeacher = roles.Find(u => u.Name == "Teacher");
                var rolStudent = roles.Find(u => u.Name == "Student");
                var rolUser = roles.Find(u => u.Name == "User");


                if (!string.IsNullOrEmpty(searchRole) && !string.IsNullOrEmpty(searchName))
                {
                    //El resultSearch permite almacenar los usuarios de acuerdo a la busqueda que se realice en la base de datos
                    //segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                    //por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                    var resultSearch = (
                        from au in _db.Users.Where(x => x.Name!.ToLower().Contains(searchName) || x.LastName!.ToLower().Contains(searchName))
                        from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                        from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Name == searchRole )
                            //from ar in _db.Roles.Where(x => x.Id == aur.RoleId)
                        select new UserDto
                        {
                            Id = au.Id,
                            Name = au.Name!,
                            LastName = au.LastName!,
                            UserName = au.UserName,
                            Phone = au.PhoneNumber,
                            Role = ar.Name,
                        }
                    ).ToList();

                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de usuarios";
                    _response.Result = resultSearch;
                    return Ok(_response);
                }

                if (!string.IsNullOrEmpty(searchRole) || !string.IsNullOrEmpty(searchName))
                {
                    if (!string.IsNullOrEmpty(searchName))
                    {
                        //El resultSearch permite almacenar los usuarios de acuerdo a la busqueda que se realice en la base de datos
                        //segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                        //por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                        var resultSearchName = (
                            from au in _db.Users.Where(x => x.Name!.ToLower().Contains(searchName) || x.LastName!.ToLower().Contains(searchName))
                            from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                            from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Id != rolStudent!.Id && x.Id != rolUser!.Id)
                                //from ar in _db.Roles.Where(x => x.Id == aur.RoleId)
                            select new UserDto
                            {
                                Id = au.Id,
                                Name = au.Name!,
                                LastName = au.LastName!,
                                UserName = au.UserName,
                                Phone = au.PhoneNumber,
                                Role = ar.Name,
                            }
                        ).ToList();

                        _response.isSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha obtenido la lista de usuarios";
                        _response.Result = resultSearchName;
                        return Ok(_response);
                    }
                    //El resultSearch permite almacenar los usuarios de acuerdo a la busqueda que se realice en la base de datos
                    //segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                    //por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                    var resultSearchRole = (
                        from au in _db.Users
                        from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                        from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Name == searchRole)
                            //from ar in _db.Roles.Where(x => x.Id == aur.RoleId)
                        select new UserDto
                        {
                            Id = au.Id,
                            Name = au.Name!,
                            LastName = au.LastName!,
                            UserName = au.UserName,
                            Phone = au.PhoneNumber,
                            Role = ar.Name,
                        }
                    ).ToList();


                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de usuarios";
                    _response.Result = resultSearchRole;
                    return Ok(_response);

                }

                

                //El result permite almacenar los usuarios de acuerdo al rol de usuario, en este caso los roles del tipo administrativo y/o colaboradores
                //en la base de datos segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                //por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                var result = (
                    from au in _db.Users
                    from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                    from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Id != rolStudent!.Id && x.Id != rolUser!.Id) //En esta linea se busca los roles con excepcion de Student y User
                    //from ar in _db.Roles.Where(x => x.Id == aur.RoleId)
                    select new UserDto
                    {
                        Id = au.Id,
                        Name = au.Name!,
                        LastName = au.LastName!,
                        UserName = au.UserName,
                        Phone = au.PhoneNumber,
                        Role = ar.Name,
                    }
                ).ToList();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la lista de usuarios";
                _response.Result = result;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };

            }

            return _response;
        }

        [HttpDelete]
        [Route("deleteUser/{id}",Name ="deleteUser")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteUser(string id)
        {
            try
            {
                var course = await _dbCourse.GetAllAsync(u => u.TeacherId == id, tracked:false);

                if (course.Count > 0 && course != null)
                {
                    foreach (var item in course)
                    {
                        Course model = new()
                        {
                            Id = item.Id,
                            TeacherId = null,
                            Codigo = item.Codigo,
                            FolderId = item.FolderId,
                            Titulo = item.Titulo,
                            Detalle = item.Detalle,
                            Precio = item.Precio,
                            //Deberes = item.Deberes,
                            //Pruebas = item.Pruebas,
                            //NotaFinal = item.NotaFinal,
                            //Capitulos = item.Capitulos,
                            ImagenUrl = item.ImagenUrl,
                            BibliotecaUrl = item.BibliotecaUrl,
                            ClaseUrl = item.ClaseUrl,
                            //IsActive = item.IsActive,
                            //State = item.State,
                            //Teacher = null
                        };
                        await _dbCourse.UpdateAsync(model);
                        await _dbCourse.SaveAsync();

                    }
                }

                //Esta seccion permitira eliminar el usuario si existe en la tabla de Teacher
                var teacher = await _db.TeacherTbl.FirstOrDefaultAsync(u => u.Id == id);
                if (teacher != null)
                {
                    _db.TeacherTbl.Remove(teacher);
                    await _db.SaveChangesAsync();

                }

                //Esta seccion permitira eliminar el usuario si existe en la tabla de Student
                var student = await _db.StudentTbl.FirstOrDefaultAsync(u => u.Id == id);
                if (student != null)
                {
                    _db.StudentTbl.Remove(student);
                    await _db.SaveChangesAsync();

                }


                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido realizar esta acción";
                    return BadRequest(_response);
                }

                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha eliminado el registro correctamente";
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
            
        }


        [HttpGet("getStudents")]
        public async Task<ActionResult<ApiResponse>> GetStudents([FromQuery] string? search)
        {
            try
            {
                var roles = await _db.Roles.ToListAsync();
                var rolStudent = roles.Find(u => u.Name == "Student");
                if (!string.IsNullOrEmpty(search))
                {
                    //El result permite almacenar los usuarios de acuerdo al rol de usuario, en este caso los roles del tipo administrativo y/o colaboradores
                    //en la base de datos segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                    //por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                    var resultSearch = (
                        from au in _db.Users.Where(x => x.Name!.ToLower().Contains(search) || x.LastName!.ToLower().Contains(search))
                        from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                        from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Id == rolStudent!.Id) //En esta linea se busca los roles con excepcion de Student y User
                                                                                                                       //from ar in _db.Roles.Where(x => x.Id == aur.RoleId)
                        select new UserDto
                        {
                            Id = au.Id,
                            Name = au.Name!,
                            LastName = au.LastName!,
                            UserName = au.UserName,
                            Phone = au.PhoneNumber,
                        }
                    ).ToList();

                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Estudiantes";
                    _response.Result = resultSearch;
                    return Ok(_response);
                }

                //El result permite almacenar los usuarios de acuerdo al rol de usuario, en este caso los roles del tipo administrativo y/o colaboradores
                //en la base de datos segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                //por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                var result = (
                    from au in _db.Users
                    from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                    from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Id == rolStudent!.Id) //En esta linea se busca los roles con excepcion de Student y User
                                                                                                 //from ar in _db.Roles.Where(x => x.Id == aur.RoleId)
                    select new UserDto
                    {
                        Id = au.Id,
                        Name = au.Name!,
                        LastName = au.LastName!,
                        UserName = au.UserName,
                        Phone = au.PhoneNumber,
                    }
                ).ToList();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la lista de Estudiantes";
                _response.Result = result;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };

            }

            return _response;
        }

        [HttpPut("assigmentCourse/{id:int}", Name ="AssigmentCourse")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> AssigmentCourse(int id,[FromBody] string teacherId)
        {
            try
            {
                //if (id != courseDto.Id)
                //{
                //    _response.isSuccess = false;
                //    _response.StatusCode = HttpStatusCode.BadRequest;
                //    _response.Message = "Ha ocurrido un problema mientras se actualizaba el registro";
                //    return BadRequest(_response);
                //}
                var course = await _dbCourse.GetAsync(u => u.Id == id, tracked:false);
                if (course == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe";
                    return BadRequest(_response);
                }

                //var capitulos = JsonConvert.SerializeObject(course.Capitulos);
                //var deberes = JsonConvert.SerializeObject(course.Deberes);
                //var pruebas = JsonConvert.SerializeObject(course.Pruebas);

                Course model = new()
                {
                    Id = course.Id,
                    TeacherId = teacherId,
                    Codigo = course.Codigo,
                    Titulo = course.Titulo,
                    Detalle = course.Detalle,
                    Precio = course.Precio,
                    FolderId = course.FolderId,
                    //Deberes = course.Deberes,
                    //Pruebas = course.Pruebas,
                    //NotaFinal = course.NotaFinal,
                    //Capitulos = course.Capitulos,
                    ImagenUrl = course.ImagenUrl,
                    //IsActive = course.IsActive,
                    //State = course.State
                    BibliotecaUrl = course.BibliotecaUrl,
                    ClaseUrl = course.ClaseUrl,
                };

                await _dbCourse.UpdateAsync(model);
                await _dbCourse.SaveAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = $"Se ha asigando al profesor al curso de {course.Titulo}";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };

            }

            return _response;
        }

        [HttpPut("deleteAssigmentCourse/{id:int}", Name = "DeleteAssigmentCourse")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteAssigmentCourse(int id, [FromBody] string teacherId)
        {
            try
            {
                //if (id != courseDto.Id)
                //{
                //    _response.isSuccess = false;
                //    _response.StatusCode = HttpStatusCode.BadRequest;
                //    _response.Message = "Ha ocurrido un problema mientras se actualizaba el registro";
                //    return BadRequest(_response);
                //}
                var course = await _dbCourse.GetAsync(u => u.Id == id, tracked: false);
                if (course == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe";
                    return BadRequest(_response);
                }

                //var capitulos = JsonConvert.SerializeObject(course.Capitulos);
                //var deberes = JsonConvert.SerializeObject(course.Deberes);
                //var pruebas = JsonConvert.SerializeObject(course.Pruebas);

                Course model = new()
                {
                    Id = course.Id,
                    TeacherId = null,
                    Titulo = course.Titulo,
                    Codigo = course.Codigo,
                    Detalle = course.Detalle,
                    Precio = course.Precio,
                    //Deberes = course.Deberes,
                    //Pruebas = course.Pruebas,
                    //NotaFinal = course.NotaFinal,
                    //Capitulos = course.Capitulos,
                    ImagenUrl = course.ImagenUrl,
                    //IsActive = course.IsActive,
                    //State = course.State
                    FolderId = course.FolderId,
                    BibliotecaUrl = course.BibliotecaUrl,
                    ClaseUrl = course.ClaseUrl,
                };

                await _dbCourse.UpdateAsync(model);
                await _dbCourse.SaveAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = $"Se ha eliminado al profesor del curso de {course.Titulo}";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };

            }

            return _response;
        }


        /// <summary>
        /// Este controlador permite obtener todos los cursos para poder observar su informacion en el front-end tanto del administrador como el de la secretaria
        /// </summary>
        /// <param name="search">Es el campo que contiene los caracteres que tengan coincidencia con el titulo del curso</param>
        /// <returns>Retorna una lista con todos los cursos y en el caso de buscar con search devuelve una lista de acuerdo con el search</returns>
        [HttpGet]
        [Route("getAllCourse")]
        public async Task<ActionResult<ApiResponse>> GetAllCourse([FromQuery] string? search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var coursesQuery = await _dbCourse.GetAllAsync(u => u.Titulo!.ToLower().Contains(search),includeProperties:"Teacher");
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Cursos";
                    _response.Result = coursesQuery;
                    return Ok(_response);
                }

                var courses = await _dbCourse.GetAllAsync(includeProperties: "Teacher");

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la lista de Cursos";
                _response.Result = courses;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;

        }


        [HttpGet]
        [Route("getComentarios")]
        public async Task<ActionResult<ApiResponse>> GetComentarios([FromQuery] string? search)
        {
            try
            {
                var comentarios = await _db.ComentarioTbl.ToListAsync();
                if(comentarios != null)
                {
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de comentarios";
                    _response.Result = comentarios;
                    return Ok(_response);
                }

                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha obtenido la lista de comentarios";
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;

        }

    }
}
