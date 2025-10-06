// <copyright file="TeacherRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Data.Models.PaypalOrder;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;
using static Google.Apis.Drive.v3.DriveService;

namespace User.Managment.Repository.Repository
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ICourseRepositoty _dbCourse;
        private readonly IMatriculaRepository _dbMatricula;
        private readonly GoogleDriveSettings _googleDriveConfig;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        // private readonly IWebHostEnvironment _hostEnvironment;
        // private readonly GoogleDriveService _googleDriveService;
        // private readonly IConfiguration _configuration;
        // protected string clientSecret;
        // protected string clientId;
        // protected string authUri;
        public TeacherRepository(ApplicationDbContext db, ICourseRepositoty dbCourse, IMatriculaRepository dbMatricula, GoogleDriveSettings googleDriveConfig, IMapper mapper)
            : base(db)
        {
            _db = db;
            _dbCourse = dbCourse;
            _dbMatricula = dbMatricula;
            _googleDriveConfig = googleDriveConfig;
            _mapper = mapper;
            this._response = new();
        }

        public async Task<ResponseDto> GetAllCourseAsync(string id, string? search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var coursesQuery = await _dbCourse.GetAllAsync(u => u.Titulo!.ToLower().Contains(search) && u.TeacherId == id, includeProperties: "Teacher");
                    if (coursesQuery != null && coursesQuery.Count > 0)
                    {
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha obtenido la lista de Cursos";
                        _response.Result = _mapper.Map<List<CourseDto>>(coursesQuery);
                        return _response;
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "No se ha encontrado coincidencias!!";
                        return _response;
                    }
                }

                var teacherCourse = await _dbCourse.GetAllAsync(u => u.TeacherId == id, includeProperties: "Teacher");
                if (teacherCourse == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado cursos asignados";
                    return _response;
                }

                var teacher = await this.GetAsync(u => u.Id == id, tracked: false);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = $"Se ha obtenido el/los curso/s asignado/s al profesor";
                _response.Result = _mapper.Map<List<CourseDto>>(teacherCourse);
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

        public async Task<ResponseDto> GetAllStudentAsync(int? cursoId, string? search, string start, string end)
        {
            try
            {
                if (string.IsNullOrEmpty(search) && cursoId == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido obtener la lista de estudiantes. Elija un curso!";
                    return _response;
                }
                else if (!string.IsNullOrEmpty(search) && start != "null" && end != "null")
                {
                    DateTime inicioDate = JsonConvert.DeserializeObject<DateTime>(start);
                    DateTime finDate = JsonConvert.DeserializeObject<DateTime>(end);
                    var studentList = await _dbMatricula.GetAllAsync(
                        u => u.CursoId == cursoId
                    && (u.Estudiante!.LastName!.Contains(search)
                    || u.Estudiante.Name!.Contains(search)
                    || u.Estudiante.Email!.Contains(search))
                    && u.FechaInscripcion >= inicioDate && u.FechaInscripcion <= finDate, tracked: false, includeProperties: "Curso,Estudiante");
                    if (studentList != null)
                    {
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha podido obtener el estudiante en específico";
                        _response.Result = studentList.OrderByDescending(x => x.FechaInscripcion);
                        return _response;
                    }

                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido obtener la lista de estudiantes!";

                    return _response;
                }
                else if (start != "null" && end != "null")
                {
                    DateTime startDate = JsonConvert.DeserializeObject<DateTime>(start);
                    DateTime endDate = JsonConvert.DeserializeObject<DateTime>(end);
                    var studentList = await _dbMatricula.GetAllAsync(
                        u => u.CursoId == cursoId
                    && u.FechaInscripcion >= startDate && u.FechaInscripcion <= endDate,
                        tracked: false, includeProperties: "Curso,Estudiante");

                    if (studentList != null)
                    {
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha podido obtener el estudiante en específico";
                        _response.Result = studentList.OrderByDescending(x => x.FechaInscripcion);
                        return _response;
                    }

                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido obtener la lista de estudiantes!";

                    return _response;
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    var studentList = await _dbMatricula.GetAllAsync(
                        u => u.CursoId == cursoId
                    && (u.Estudiante!.LastName!.Contains(search)
                    || u.Estudiante.Name!.Contains(search)
                    || u.Estudiante.Email!.Contains(search)), tracked: false, includeProperties: "Curso,Estudiante");
                    if (studentList != null)
                    {
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha podido obtener el estudiante en específico";
                        _response.Result = studentList.OrderByDescending(x => x.FechaInscripcion);
                        return _response;
                    }

                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido obtener la lista de estudiantes!";

                    return _response;
                }
                else
                {
                    var studentList = await _dbMatricula.GetAllAsync(u => u.CursoId == cursoId, tracked: false, includeProperties: "Curso,Estudiante");
                    if (studentList != null)
                    {
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha podido obtener el estudiante en específico";
                        _response.Result = studentList.OrderByDescending(x => x.FechaInscripcion);
                        return _response;
                    }

                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido obtener la lista de estudiantes!";
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllTeacherAsync()
        {
            try
            {
                var teacherlist = await this.GetAllAsync(tracked: false);
                if (teacherlist == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "No se obtuvieron resultados";
                    _response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.Message = "Se obtuvo la lista de profesores satisfactoriamente";
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = _mapper.Map<List<TeacherDto>>(teacherlist);
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

        public async Task<ResponseDto> GetTeacherAsync(string id)
        {
            try
            {
                var teacher = await this.GetAsync(u => u.Id == id);
                if (teacher == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "No se encontraron registros de este usuario!!!";
                    _response.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.Message = "Se obtuvo el registro del usuario";
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = _mapper.Map<TeacherDto>(teacher);
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

        public async Task<ResponseDto> UpdateGradeMatriculaAsync(int id, string? notaFinal)
        {
            try
            {
                var matriculaExist = await _dbMatricula.GetAsync(u => u.Id == id, tracked: false);
                if (matriculaExist != null && !string.IsNullOrEmpty(notaFinal))
                {
                    Matricula model = new()
                    {
                        Id = matriculaExist.Id,
                        FechaInscripcion = matriculaExist.FechaInscripcion,
                        CursoId = matriculaExist.CursoId,
                        EstudianteId = matriculaExist.EstudianteId,
                        IsActive = matriculaExist.IsActive,
                        Estado = "Completado",
                        NotaFinal = Convert.ToDouble(notaFinal),
                        CertificadoId = matriculaExist.CertificadoId,
                    };

                    _db.MatriculaTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha colocado la nota final de la matrícula del estudiante!!";
                    _response.Result = model;
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido colocar la nota final en la matrícula del estudiante. Asegúrate de colocar la nota";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateGradeTaskAsync(int? id, string studentId, string? calificacion)
        {
            try
            {
                var notaDeberExist = await _db.NotaDeberTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id && u.StudentId == studentId);
                if (notaDeberExist != null && !string.IsNullOrEmpty(calificacion))
                {
                    NotaDeber model = new()
                    {
                        Id = notaDeberExist.Id,
                        Observacion = notaDeberExist.Observacion,
                        Estado = notaDeberExist.Estado,
                        Calificacion = Convert.ToDouble(calificacion),
                        DeberId = notaDeberExist.DeberId,
                        StudentId = notaDeberExist.StudentId,
                        FileUrl = notaDeberExist.FileUrl,
                    };

                    _db.NotaDeberTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha calificado el deber con éxito!!";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido calificar el deber!!";
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

        public async Task<ResponseDto> UpsertGradeTestAsync(int id, string studentId, string? calificacion)
        {
            try
            {
                var notaPruebaDto = await _db.NotaPruebaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.PruebaId == id && u.StudentId == studentId);
                if (notaPruebaDto != null && !string.IsNullOrEmpty(calificacion))
                {
                    NotaPrueba model = new()
                    {
                        Id = notaPruebaDto.Id,
                        Observacion = notaPruebaDto.Observacion,
                        Estado = notaPruebaDto.Estado,
                        Calificacion = Convert.ToDouble(calificacion),
                        PruebaId = notaPruebaDto.PruebaId,
                        StudentId = notaPruebaDto.StudentId,
                    };

                    _db.NotaPruebaTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha actualizado la calificación de la prueba con éxito!!";
                    return _response;
                }
                else if (!string.IsNullOrEmpty(calificacion))
                {
                    NotaPrueba model = new()
                    {
                        Observacion = "Revisado",
                        Estado = "Calificado",
                        Calificacion = Convert.ToDouble(calificacion),
                        PruebaId = id,
                        StudentId = studentId,
                    };

                    await _db.NotaPruebaTbl.AddAsync(model);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha calificado la prueba con éxito!!";
                    return _response;
                }

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha podido calificar la prueba. Revise la prueba antes de calificar y coloca la nota correspondiente";
                return _response;

                // _response.IsSuccess = false;
                // _response.StatusCode = HttpStatusCode.BadRequest;
                // _response.Message = "No se ha podido entregar el deber. Inténtelo nuevamente!";
                // return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateImageTeacherAsync(string id, IFormFile? file)
        {
            try
            {
                var teacherDto = await this.GetAsync(u => u.Id == id, tracked: false);
                if (teacherDto == null || teacherDto.Id != id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido completar con la actualización de tu fotografía!";
                    return _response;
                }

                if (file != null)
                {
                    var service = GetService(); // Se inicia sesion para enviar o eliminar archivos en google drive

                    // En caso de que exista el identificador del archivo se procede a eliminarlo de google drive para poder almacenar otro
                    if (teacherDto.PhotoURL != null)
                    {
                        DeleteFile(service, teacherDto.PhotoURL);
                    }

                    // Permite almacenar el idFile creado en google drive para almacenarlo y utilizarlo en la aplicacion
                    // Este link es el identificador de la anterior carpeta para almacenar las fotos de perfil del usuario prefesor : 1PuD7eY7zNN1kVs4v0-bD6t9_XDFJfGFa
                    // Estos links estan vinculados a las carpetas de Drive de google por lo que se debe revisar su forderId en google drive
                    string idFile = await UploadFile(service, file, "1PuD7eY7zNN1kVs4v0-bD6t9_XDFJfGFa");

                    Teacher modelWithPhoto = new()
                    {
                        Id = teacherDto.Id,
                        Name = teacherDto.Name,
                        LastName = teacherDto.LastName,
                        Phone = teacherDto.Phone,
                        Biografy = teacherDto.Biografy,
                        Email = teacherDto.Email,
                        PhotoURL = idFile,
                    };

                    _db.TeacherTbl.Update(modelWithPhoto);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Tu fotografía se ha actualizado correctamente!";
                    return _response;
                }

                /*Teacher model = new()
                //{
                //    Id = teacherDto.Id,
                //    Name = teacherDto.Name,
                //    LastName = teacherDto.LastName,
                //    Phone = teacherDto.Phone,
                //    Biografy = teacherDto.Biografy,
                //    Email = teacherDto.Email,
                //    PhotoURL = teacherDto.PhotoURL
                //};

                //_db.TeacherTbl.Update(model);
                //await _db.SaveChangesAsync();

                //_response.IsSuccess = true;
                //_response.StatusCode = HttpStatusCode.OK;
                //_response.Message = "Su información se ha actualizado correctamente!";
                //return Ok(_response);*/

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha podido actualizar tu fotografía!";
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

        public async Task<ResponseDto> UpdateMatriculaStateAsync(int id, string studentId, bool isActive)
        {
            try
            {
                var matriculaExist = await _dbMatricula.GetAsync(u => u.Id == id && u.EstudianteId == studentId, tracked: false);
                if (matriculaExist == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido deshabilitar al matrícula del estudiante!!";
                    return _response;
                }
                else if (isActive)
                {
                    Matricula model = new()
                    {
                        Id = matriculaExist.Id,
                        CursoId = matriculaExist.CursoId,
                        FechaInscripcion = matriculaExist.FechaInscripcion,
                        EstudianteId = matriculaExist.EstudianteId,
                        IsActive = false,
                        Estado = matriculaExist.Estado,
                        NotaFinal = matriculaExist.NotaFinal,
                        CertificadoId = matriculaExist.CertificadoId,
                    };

                    _db.MatriculaTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha deshabilitado la matrícula del estudiante!!";
                    return _response;
                }
                else
                {
                    Matricula model = new()
                    {
                        Id = matriculaExist.Id,
                        CursoId = matriculaExist.CursoId,
                        FechaInscripcion = matriculaExist.FechaInscripcion,
                        EstudianteId = matriculaExist.EstudianteId,
                        IsActive = true,
                        Estado = matriculaExist.Estado,
                        NotaFinal = matriculaExist.NotaFinal,
                        CertificadoId = matriculaExist.CertificadoId,
                    };

                    _db.MatriculaTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha habilitado la matrícula del estudiante!!";
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateTeacherAsync(string id, TeacherDto teacherDto)
        {
            try
            {
                if (teacherDto.Id != id || teacherDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error en el sistema. Inténtelo nuevamente!";
                    return _response;
                }

                var teacher = await this.GetAsync(u => u.Id == id, tracked: false);
                if (teacher == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El usuario no está registrado!";
                    return _response;
                }

                _db.TeacherTbl.Update(_mapper.Map<Teacher>(teacherDto));
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Tu información se ha actualizado correctamente!";
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

        /// <summary>
        /// Esta funcion permite sincronizar las credenciales obtenidas para enlazar el proyecto .net con google drive
        /// Si se necesita informacion de como realizarlo se adjunta el link de instrucciones: https://medium.com/geekculture/upload-files-to-google-drive-with-c-c32d5c8a7abc.
        /// </summary>
        /// <returns>Se retorna el inicio de sesion de la aplicacion con google drive.</returns>
        /// private static DriveService GetService()
        private DriveService GetService()
        {
            var tokenResponse = new TokenResponse
            {
                AccessToken = _googleDriveConfig.AccessToken,
                RefreshToken = _googleDriveConfig.RefreshToken,
            };

            var applicationName = _googleDriveConfig.ApplicationName;
            var userName = _googleDriveConfig.UserName;

            var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _googleDriveConfig.ClientId,
                    ClientSecret = _googleDriveConfig.ClientSecret,
                },
                Scopes = new[] { Scope.Drive },
                DataStore = new FileDataStore(applicationName),
            });

            var credential = new UserCredential(apiCodeFlow, userName, tokenResponse);

            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

            return service;
        }

        /// <summary>
        /// Esta funcion permite cargar un archivo en google drive
        /// Entre estos archivos que se pueden cargar son documentos, fotos y audios, pero no videos.
        /// </summary>
        /// <param name="service">Es la sesion que se abrio para enlazar la aplicacion con google drive.</param>
        /// <param name="file">Es el archivo que se va a subir que puede ser foto, documentos o musica.</param>
        /// <param name="idFolder">Es el identificador de la carpeta donde se va a subir el archivo para este caso es la carpeta PruebaCapernova en Google Drive.</param>
        /// <returns>Retorna el IdFile que se creo en google drive.</returns>
        private async Task<string> UploadFile(DriveService service, IFormFile file, string idFolder)
        {
            string fileMime = file.ContentType;
            var driveFile = new Google.Apis.Drive.v3.Data.File
            {
                Name = file.FileName,
                MimeType = file.ContentType,
                Parents = new string[] { idFolder },
            };

            var request = service.Files.Create(driveFile, file.OpenReadStream(), fileMime); // OpenReadStream permite abrir el archivo para enviarlo al servicio de Google Drive

            // request.Fields permite que se generen los campos que queremos obtener informacion como el id, webViewLink, etc. Vease os campos que tiene en el ResponseBody.{Fields}
            request.Fields = "id, webViewLink"; // Se agrego webViewlink para obtener el link de enlace

            var response = await request.UploadAsync();

            return response.Status != UploadStatus.Completed ? throw response.Exception : request.ResponseBody.Id;
        }

        /// <summary>
        /// Esta función permite eliminar un archivo con el IdFile que se encuentre en el google drive.
        /// </summary>
        /// <param name="service">Es la sesion que se abrio para enlazar la aplicacion con google drive.</param>
        /// <param name="idFile">Es el identificador del archivo a eliminar.</param>
        private async void DeleteFile(DriveService service, string idFile)
        {
            var command = service.Files.Delete(idFile);
            var result = await command.ExecuteAsync();
        }
    }
}