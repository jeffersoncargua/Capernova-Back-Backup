// <copyright file="ManagmentRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Authentication.SignUp;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Data.Models.PaypalOrder;
using User.Managment.Data.Models.PaypalOrder.Dto;
using User.Managment.Data.Models.Student;
using User.Managment.Data.Models.Student.DTO;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class ManagmentRepository : IManagmentRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // private readonly SignInManager<ApplicationUser> _signInManager; // Permite controlar el envio del codigo OTP
        private readonly IEmailRepository _emailRepository;
        private readonly ApplicationDbContext _db;
        private readonly IMatriculaRepository _dbMatricula;
        private readonly ICourseRepositoty _dbCourse;
        private readonly ITeacherRepository _dbTeacher;
        private readonly IStudentRepository _dbStudent;
        private readonly IMapper _mapper;
        private readonly FrontSettings _frontURL;
        protected ResponseDto _response;

        public ManagmentRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager,
            IEmailRepository emailRepository, ApplicationDbContext db, IMatriculaRepository dbMatricula, ICourseRepositoty dbCourse,
            FrontSettings frontURL, ITeacherRepository dbTeacher, IStudentRepository dbStudent, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            // _signInManager = signInManager;
            _emailRepository = emailRepository;
            _db = db;
            _frontURL = frontURL;
            _dbMatricula = dbMatricula;
            _dbCourse = dbCourse;
            _dbTeacher = dbTeacher;
            _dbStudent = dbStudent;
            _mapper = mapper;
            this._response = new();
        }

        public async Task<ResponseDto> AssigmentCourseAsync(int id, string teacherId)
        {
            try
            {
                var course = await _dbCourse.GetAsync(u => u.Id == id, tracked: false);
                if (course == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe";
                }
                else
                {
                    Course model = new()
                    {
                        Id = course.Id,
                        TeacherId = teacherId,
                        Codigo = course.Codigo,
                        Titulo = course.Titulo,
                        Detalle = course.Detalle,
                        Precio = course.Precio,
                        FolderId = course.FolderId,

                        // Deberes = course.Deberes,
                        // Pruebas = course.Pruebas,
                        // NotaFinal = course.NotaFinal,
                        // Capitulos = course.Capitulos,
                        ImagenUrl = course.ImagenUrl,

                        // IsActive = course.IsActive,
                        // State = course.State
                        BibliotecaUrl = course.BibliotecaUrl,
                        ClaseUrl = course.ClaseUrl,
                    };

                    _db.CourseTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = $"Se ha asigando al profesor al curso de {course.Titulo}";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> CreateMatriculaAsync(MatriculaDto matriculaDto)
        {
            try
            {
                if (matriculaDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error. No se pudo resgistrar la matrícula";
                    return _response;
                }

                var userSystem = await _db.Users.FirstOrDefaultAsync(u => u.Id == matriculaDto.EstudianteId);
                if (userSystem == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error. No se pudo resgistrar la matrícula del estudiante";
                    return _response;
                }

                var userStudentExist = await _db.StudentTbl.FirstOrDefaultAsync(u => u.Id == matriculaDto.EstudianteId);
                if (userStudentExist == null)
                {
                    await _userManager.RemoveFromRoleAsync(userSystem, "User");

                    if (await _roleManager.RoleExistsAsync("Student"))
                    {
                        await _userManager.AddToRoleAsync(userSystem, "Student");
                        Student student = new()
                        {
                            Id = userSystem.Id,
                            Name = userSystem.Name!,
                            LastName = userSystem.LastName!,
                            Email = userSystem.Email,
                            Phone = userSystem.PhoneNumber,
                        };

                        await _dbStudent.CreateAsync(student);
                    }
                }

                var matriculaExist = await _db.MatriculaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.EstudianteId == matriculaDto.EstudianteId && u.CursoId == matriculaDto.CursoId);
                if (matriculaExist != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error. Ya tiene registrado la matrícula";
                    return _response;
                }

                var cursoExist = await _db.CourseTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == matriculaDto.CursoId);
                if (cursoExist == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error. No existe el curso ha registrar!!!";
                    return _response;
                }

                Matricula matricula = new()
                {
                    CursoId = cursoExist.Id,
                    EstudianteId = matriculaDto.EstudianteId,
                    FechaInscripcion = DateTime.Now,
                    IsActive = true,
                    Estado = "En progreso",
                };

                await _dbMatricula.CreateAsync(matricula);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha registrado la matrícula con éxito!!!";
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> DeleteAssigmentCourseAsync(int id, string teacherId)
        {
            try
            {
                var course = await _dbCourse.GetAsync(u => u.Id == id, tracked: false);
                if (course == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe";
                }
                else
                {
                    Course model = new()
                    {
                        Id = course.Id,
                        TeacherId = null,
                        Titulo = course.Titulo,
                        Codigo = course.Codigo,
                        Detalle = course.Detalle,
                        Precio = course.Precio,

                        // Deberes = course.Deberes,
                        // Pruebas = course.Pruebas,
                        // NotaFinal = course.NotaFinal,
                        // Capitulos = course.Capitulos,
                        ImagenUrl = course.ImagenUrl,

                        // IsActive = course.IsActive,
                        // State = course.State
                        FolderId = course.FolderId,
                        BibliotecaUrl = course.BibliotecaUrl,
                        ClaseUrl = course.ClaseUrl,
                    };

                    _db.CourseTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = $"Se ha eliminado al profesor del curso de {course.Titulo}";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> DeleteMatriculaAsync(int id)
        {
            try
            {
                var matriculaExist = await _dbMatricula.GetAsync(u => u.Id == id, tracked: false);
                if (matriculaExist == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error. No pudo eliminar el registro!!!";
                }
                else
                {
                    await _dbMatricula.RemoveAsync(matriculaExist);

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se eliminó el registro con éxito!!!";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> DeleteUser(string id)
        {
            try
            {
                // Este apartado solo permite revisar si el usuario a eliminar es un usario Teacher y si es verdadero
                // solo se actualiza los cursos que este usuario dictaba, caso contrario realiza las demás operaciones
                var course = await _dbCourse.GetAllAsync(u => u.TeacherId == id, tracked: false);

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

                            // Deberes = item.Deberes,
                            // Pruebas = item.Pruebas,
                            // NotaFinal = item.NotaFinal,
                            // Capitulos = item.Capitulos,
                            ImagenUrl = item.ImagenUrl,
                            BibliotecaUrl = item.BibliotecaUrl,
                            ClaseUrl = item.ClaseUrl,

                            // IsActive = item.IsActive,
                            // State = item.State,
                            // Teacher = null
                        };

                        _db.CourseTbl.Update(model);
                        await _db.SaveChangesAsync();
                    }
                }

                // Esta seccion permitira eliminar el usuario si existe en la tabla de Teacher
                var teacher = await _dbTeacher.GetAsync(u => u.Id == id, tracked: false);
                if (teacher != null)
                {
                    await _dbTeacher.RemoveAsync(teacher);
                }

                // Esta seccion permitira eliminar el usuario si existe en la tabla de Student
                var student = await _dbStudent.GetAsync(u => u.Id == id, tracked: false);
                if (student != null)
                {
                    await _dbStudent.RemoveAsync(student);
                }

                // Este apartado permitirá eliminar el usuario si existe en la base de datos agreado con identity
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido realizar esta acción";
                }
                else
                {
                    _db.Users.Remove(user);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha eliminado el registro correctamente";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllCommentaries(string? search)
        {
            try
            {
                var comentarios = await _db.ComentarioTbl.ToListAsync();
                if (comentarios != null)
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de comentarios";
                    _response.Result = _mapper.Map<List<ComentarioDto>>(comentarios);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha obtenido la lista de comentarios";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllCourse(string? search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var coursesQuery = await _dbCourse.GetAllAsync(u => u.Titulo!.ToLower().Contains(search), includeProperties: "Teacher");
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Cursos";
                    _response.Result = _mapper.Map<List<CourseDto>>(coursesQuery);
                }
                else
                {
                    var courses = await _dbCourse.GetAllAsync(includeProperties: "Teacher");

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Cursos";
                    _response.Result = _mapper.Map<List<CourseDto>>(courses);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllStudent(string? search)
        {
            try
            {
                var roles = await _db.Roles.ToListAsync();
                var rolStudent = roles.Find(u => u.Name == "Student");
                if (!string.IsNullOrEmpty(search))
                {
                    // El result permite almacenar los usuarios de acuerdo al rol de usuario, en este caso los roles del tipo administrativo y/o colaboradores
                    // en la base de datos segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                    // por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                    var resultSearch = (
                        from au in _db.Users.Where(x => x.Name!.ToLower().Contains(search) || x.LastName!.ToLower().Contains(search))
                        from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                        from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Id == rolStudent!.Id) // En esta linea se busca los roles con excepcion de Student y User
                        select new UserDto
                        {
                            Id = au.Id,
                            Name = au.Name!,
                            LastName = au.LastName!,
                            UserName = au.UserName,
                            Phone = au.PhoneNumber,
                        }
                    ).ToList();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Estudiantes";
                    _response.Result = resultSearch;
                }
                else
                {
                    // El result permite almacenar los usuarios de acuerdo al rol de usuario, en este caso los roles del tipo administrativo y/o colaboradores
                    // en la base de datos segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                    // por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                    var result = (
                        from au in _db.Users
                        from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                        from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Id == rolStudent!.Id) // En esta linea se busca los roles con excepcion de Student y User
                        select new UserDto
                        {
                            Id = au.Id,
                            Name = au.Name!,
                            LastName = au.LastName!,
                            UserName = au.UserName,
                            Phone = au.PhoneNumber,
                        }
                    ).ToList();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Estudiantes";
                    _response.Result = result;
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetMatriculaAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error. No se pudo buscar el registro!!";
                }
                else
                {
                    var matriculaList = await _dbMatricula.GetAllAsync(u => u.EstudianteId == userId, tracked: false, includeProperties: "Curso");

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de matriculas del estudiante";
                    _response.Result = matriculaList;
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetTalent(string? searchRole, string? searchName)
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
                    // El resultSearch permite almacenar los usuarios de acuerdo a la busqueda que se realice en la base de datos
                    // segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                    // por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                    var resultSearch = (
                        from au in _db.Users.Where(x => x.Name!.ToLower().Contains(searchName) || x.LastName!.ToLower().Contains(searchName))
                        from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                        from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Name == searchRole)
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

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de usuarios";
                    _response.Result = resultSearch;
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchRole) || !string.IsNullOrEmpty(searchName))
                    {
                        if (!string.IsNullOrEmpty(searchName))
                        {
                            // El resultSearch permite almacenar los usuarios de acuerdo a la busqueda que se realice en la base de datos
                            // segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                            // por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                            var resultSearchName = (
                                from au in _db.Users.Where(x => x.Name!.ToLower().Contains(searchName) || x.LastName!.ToLower().Contains(searchName))
                                from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                                from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Id != rolStudent!.Id && x.Id != rolUser!.Id)
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

                            _response.IsSuccess = true;
                            _response.StatusCode = HttpStatusCode.OK;
                            _response.Message = "Se ha obtenido la lista de usuarios";
                            _response.Result = resultSearchName;
                        }
                        else
                        {
                            // El resultSearch permite almacenar los usuarios de acuerdo a la busqueda que se realice en la base de datos
                            // segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                            // por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                            var resultSearchRole = (
                                from au in _db.Users
                                from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                                from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Name == searchRole)
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

                            _response.IsSuccess = true;
                            _response.StatusCode = HttpStatusCode.OK;
                            _response.Message = "Se ha obtenido la lista de usuarios";
                            _response.Result = resultSearchRole;
                        }
                    }
                    else
                    {
                        // El result permite almacenar los usuarios de acuerdo al rol de usuario, en este caso los roles del tipo administrativo y/o colaboradores
                        // en la base de datos segun lo requiera el usuario del front-end. En este caso solo filtra la informacion de un tipo de rol en especifico
                        // por lo tanto se realiza inner joins para poder encontrar la informacion que se necesita para enviar al front-end
                        var result = (
                            from au in _db.Users
                            from aur in _db.UserRoles.Where(x => x.UserId == au.Id)
                            from ar in _db.Roles.Where(x => x.Id == aur.RoleId && x.Id != rolStudent!.Id && x.Id != rolUser!.Id) // En esta linea se busca los roles con excepcion de Student y User
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

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha obtenido la lista de usuarios";
                        _response.Result = result;
                    }
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetUserAsync(string search)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error. No se pudo buscar el registro!!";
                }
                else
                {
                    var userExist = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == search);
                    if (userExist == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "No existen registros de tu búsqueda!!";
                    }
                    else
                    {
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha localizado el usuario";
                        _response.Result = userExist;
                    }
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> Register(RegisterUserDto registerUser)
        {
            try
            {
                string textMessage = "";

                // Se chequea si el usuario existe
                var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
                if (userExist != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El usuario ya está registrado";
                    _response.IsSuccess = false;
                }
                else
                {
                    // Se prepara el user con la informacion que se va almacenar en la base de datos
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

                    // Se consulta si el rol existe en la base de datos, esto para poder asignarle uno existente
                    // string role = "User";
                    if (await _roleManager.RoleExistsAsync(registerUser.Role))
                    {
                        // Se almacena en la base de datos
                        var result = await _userManager.CreateAsync(user, registerUser.Password);
                        if (!result.Succeeded)
                        {
                            // List<string> errores = new List<string>();
                            foreach (var error in result.Errors)
                            {
                                _response.Errors.Add(error.Description);
                            }

                            // return StatusCode(StatusCodes.Status500InternalServerError,
                            //    new Response { Status = "Error", Message =  errores.First() });
                            _response.IsSuccess = false;
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.Message = _response.Errors.FirstOrDefault()!;
                        }

                        // Se le asigna el rol al usuario
                        await _userManager.AddToRoleAsync(user, registerUser.Role);

                        // Se agrega el token para verificar el email
                        var tokenEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        // Se genera el link para enviar el token y el usuario
                        // var confirmationLink = $"https://localhost:3000/confirmEmail?token={tokenEmail}&email={user.Email}";
                        var confirmationLink = $"{_frontURL.Url}/confirmEmail?token={tokenEmail}&email={user.Email}";

                        // Se arma el mensaje que se le va a enviar el talento humano de Capernova
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

                        // Se arma el mensaje con el email del usuario registrado y el enlace de confirmacion del correo
                        var message = new Message(new string[] { user.Email! }, "Enlace de confirmación de Talento Humano de Capernova ", textMessage);

                        // Se envia el mensaje que se va a remitir por correo electrónico
                        _emailRepository.SendEmail(message);

                        if (registerUser.Role == "Teacher")
                        {
                            Teacher teacher = new()
                            {
                                Id = user.Id,
                                Name = user.Name,
                                LastName = user.LastName,
                                Email = user.Email,
                                Phone = user.PhoneNumber,
                            };

                            await _db.TeacherTbl.AddAsync(teacher);
                            await _db.SaveChangesAsync();
                        }

                        _response.StatusCode = HttpStatusCode.Created;
                        _response.IsSuccess = true;
                        _response.Message = "El usuario ha sido registrado y se ha enviado un correo para su confirmación";
                    }
                    else
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.Message = "Debe seleccionar un rol de usuario!!";
                    }
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}