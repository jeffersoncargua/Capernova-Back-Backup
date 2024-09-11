//using Google.Apis.Auth.AspNetCore3;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Drive.v3;
//using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Microsoft.OpenApi.Writers;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Repository.Repository.IRepository;
using static Google.Apis.Drive.v3.DriveService;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ICourseRepositoty _dbCourse;
        private readonly IMatriculaRepository _dbMatricula;
        //private readonly IWebHostEnvironment _hostEnvironment;
        //private readonly GoogleDriveService _googleDriveService;
        private readonly IConfiguration _configuration;
        protected ApiResponse _response;
        protected string clientSecret;
        protected string clientId;
        protected string authUri;

        public TeacherController(ApplicationDbContext db, ICourseRepositoty dbCourse, IMatriculaRepository dbMatricula, IConfiguration configuration)
        {
            _db = db;
            _dbCourse = dbCourse;    
            _dbMatricula = dbMatricula;
            //_hostEnvironment = hostEnvironment;
            //_googleDriveService = googleDriveService;
            _configuration = configuration;
            this.clientId = _configuration["GoogleDrive:ClientId"]; //permite obtener del archivo appsettings.json el clientId de google drive
            this.clientSecret = _configuration["GoogleDrive:ClientSecret"]; //permite obtener del archivo appsettings.json el redirectUri de google drive
            this.authUri = _configuration["GoogleDrive:RedirectUri"]; //permite obtener del archivo appsettings.json el redirectUri de ggolge drive
            this._response = new();
        }

        [HttpGet("getAllTeacher")]
        public async Task<ActionResult<ApiResponse>> GetAllTeacher()
        {
            try
            {
                var teacherlist = await _db.TeacherTbl.ToListAsync();
                if(teacherlist == null)
                {
                    _response.isSuccess = false;
                    _response.Message = "No se obtuvieron resultados";
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.isSuccess = true;
                _response.Message = "Se obtuvo la lista de profesores satisfactoriamente";
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = teacherlist;
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("getAllCourse")]
        public async Task<ActionResult<ApiResponse>> GetAllCourse([FromQuery]string id, [FromQuery] string? search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var coursesQuery = await _dbCourse.GetAllAsync(u => u.Titulo.ToLower().Contains(search) && u.TeacherId == id,includeProperties:"Teacher");
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Cursos";
                    _response.Result = coursesQuery;
                    return Ok(_response);
                }

                var teacherCourse = await _dbCourse.GetAllAsync(u => u.TeacherId == id,includeProperties:"Teacher");
                if (teacherCourse == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Message = "No se han encontrado cursos asiganados";
                    return BadRequest(_response);
                }

                var teacher = await _db.TeacherTbl.FirstOrDefaultAsync(u => u.Id == id);
                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = $"Se ha obtenido la lista de cursos asignados al profesor";
                _response.Result = teacherCourse;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;

        }



        [HttpPut("updateTeacher", Name = "updateTeacher")]
        public async Task<ActionResult<ApiResponse>> UpdateTeacher([FromQuery]string id, [FromBody] TeacherDto teacherDto)
        {
            try
            {
                if (teacherDto.Id != id || teacherDto == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error en el sistema, intentelo nuevamente!";
                    return BadRequest(_response);
                }


                var teacher = await _db.TeacherTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (teacher == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El usuario no esta registrado!";
                    return BadRequest(_response);
                }

               

                Teacher model = new()
                {
                    Id = teacherDto.Id,
                    Name = teacherDto.Name,
                    LastName = teacherDto.LastName,
                    Phone = teacherDto.Phone,
                    Biografy = teacherDto.Biografy,
                    Email = teacherDto.Email,
                    PhotoURL = teacherDto.PhotoURL
                };

                _db.TeacherTbl.Update(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Su información se ha actualizado correctamente!";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("updateImageTeacher",Name = "updateImageTeacher")]
        //[GoogleScopedAuthorize(DriveService.ScopeConstants.DriveReadonly)]
        public async Task<ActionResult<ApiResponse>> UpdateImageTeacher([FromQuery] string id, IFormFile? file)
        {
            try
            {
                var teacherDto = await _db.TeacherTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if(teacherDto == null || teacherDto.Id != id)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido realizar su solicitud!";
                    return BadRequest(_response);
                }

                if (file != null)
                {
                    var service = GetService(); //Se inicia sesion para enviar o eliminar archivos en google drive

                    //En caso de que exista el identificador del archivo se procede a eliminarlo de google drive para poder almacenar otro
                    if (teacherDto.PhotoURL != null)
                    {
                        DeleteFile(service, teacherDto.PhotoURL); 
                    }

                    //Permite almacenar el idFile creado en google drive para almacenarlo y utilizarlo en la aplicacion  
                    string idFile = await UploadFile(service, file, "1PuD7eY7zNN1kVs4v0-bD6t9_XDFJfGFa");

                    Teacher modelWithPhoto = new()
                    {
                        Id = teacherDto.Id,
                        Name = teacherDto.Name,
                        LastName = teacherDto.LastName,
                        Phone = teacherDto.Phone,
                        Biografy = teacherDto.Biografy,
                        Email = teacherDto.Email,
                        PhotoURL = idFile
                    };

                    _db.TeacherTbl.Update(modelWithPhoto);
                    //_db.TeacherTbl.Update(teacher);
                    await _db.SaveChangesAsync();

                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Su fotografía se ha actualizado correctamente!";
                    return Ok(_response);

                }

                //Teacher model = new()
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

                //_response.isSuccess = true;
                //_response.StatusCode = HttpStatusCode.OK;
                //_response.Message = "Su información se ha actualizado correctamente!";
                //return Ok(_response);

                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha podido actualizar su fotografía!";
                return BadRequest(_response);

            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;

        }


        [HttpGet("getStudents",Name = "getStudents")]
        public async Task<ActionResult<ApiResponse>> GetStudents([FromQuery] int? cursoId,[FromQuery] string? search)
        {
            try
            {
                if (string.IsNullOrEmpty(search) && cursoId == 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido obtener la lista de estudiantes. Elija un curso!";
                    return BadRequest(_response);
                }else if (!string.IsNullOrEmpty(search))
                {
                    var studentList = await _dbMatricula.GetAllAsync(u => u.CursoId == cursoId && (u.Estudiante.LastName.Contains(search) || u.Estudiante.Name.Contains(search) || u.Estudiante.Email.Contains(search)), tracked: false, includeProperties: "Curso,Estudiante");
                    if (studentList != null) 
                    {
                        _response.isSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha podido obtener el estudiante en especifico";
                        _response.Result = studentList;
                        return Ok(_response);
                    }

                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido obtener la lista de estudiantes!";

                    return BadRequest(_response);
                }
                else
                {
                    var studentList = await _dbMatricula.GetAllAsync(u => u.CursoId == cursoId, tracked: false, includeProperties: "Curso,Estudiante");
                    if (studentList != null)
                    {
                        _response.isSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha podido obtener el estudiante en especifico";
                        _response.Result = studentList;
                        return Ok(_response);
                    }
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido obtener la lista de estudiantes!";
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

        [HttpPut("updateNotaDeber", Name = "updateNotaDeber")]
        //[GoogleScopedAuthorize(DriveService.ScopeConstants.DriveReadonly)]
        public async Task<ActionResult<ApiResponse>> UpdateNotaDeber([FromQuery] int? id, [FromQuery] string studentId, [FromBody] string? calificacion)
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
                        FileUrl= notaDeberExist.FileUrl
                    };

                    _db.NotaDeberTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha calificado el deber con exito!!";
                    return Ok(_response);
                }

                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha podido calificar el deber!!";
                return BadRequest(_response);

            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;

        }


        [HttpPut("upsertNotaPrueba", Name = "upsertNotaPrueba")]
        //[GoogleScopedAuthorize(DriveService.ScopeConstants.DriveReadonly)]
        public async Task<ActionResult<ApiResponse>> UpsertNotaPrueba([FromQuery] int id, [FromQuery] string studentId, [FromBody] string? calificacion)
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

                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha actualizado la calificación de la prueba con exito!!";
                    return Ok(_response);
                }
                else if(!string.IsNullOrEmpty(calificacion))
                {
                    NotaPrueba model = new()
                    {
                        Observacion = "Revisado",
                        Estado = "Calificado",
                        Calificacion = Convert.ToDouble(calificacion),
                        PruebaId = id,
                        StudentId = studentId
                    };

                    await _db.NotaPruebaTbl.AddAsync(model);
                    await _db.SaveChangesAsync();

                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha calificado la prueba con exito!!";
                    return Ok(_response);
                }

                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha podido calificar la prueba!!";
                return BadRequest(_response);

                //_response.isSuccess = false;
                //_response.StatusCode = HttpStatusCode.BadRequest;
                //_response.Message = "No se ha podido entregar el deber. Inténtelo nuevamente!";
                //return BadRequest(_response);

            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;

        }


        /// <summary>
        /// Esta funcion permite sincronizar las credenciales obtenidas para enlazar el proyecto .net con google drive
        /// Si se necesita informacion de como realizarlo se adjunta el link de instrucciones: https://medium.com/geekculture/upload-files-to-google-drive-with-c-c32d5c8a7abc
        /// </summary>
        /// <returns>Se retorna el inicio de sesion de la aplicacion con google drive</returns>
        private static DriveService GetService()
        {
            var tokenResponse = new TokenResponse
            {
                AccessToken = "ya29.a0AcM612wbbA-WaD2EfhrSlham0ROey3qfGH97v--LiqXGTpDblLiATf0uqr6u9PH47s6C9-M8M1AavVH9ZntXLka320FZihjyQkB29pyGJYzrWE8FiMcUcSYhbcTXvPXsYARxYrdLIQew_t80xsia_RCtqUc6i316tu2Ugk-aaCgYKAcQSARASFQHGX2MiqZhMRTqpPtkiYYRQPew9-w0175",
                RefreshToken = "1//04ZiMEftaDgeqCgYIARAAGAQSNwF-L9IrQ1GglPJz6swCd375YH2nHYnUqlhMOnN103qpP5QARmX7IuMtmt6av1hxVmPLMiJgExk",
            };

            var applicationName = "CapernovaTest";
            var userName = "jeffersoncargua@gmail.com";

            var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = "533765406103-0mt3gsdbckirrrk7920dsdn0552fmkoe.apps.googleusercontent.com",
                    ClientSecret = "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"
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
        /// <param name="service">Es la sesion que se abrio para enlazar la aplicacion con google drive</param>
        /// <param name="file">Es el archivo que se va a subir que puede ser foto, documentos o musica</param>
        /// <param name="idFolder">Es el identificador de la carpeta donde se va a subir el archivo para este caso es la carpeta PruebaCapernova en Google Drive</param>
        /// <returns>Retorna el IdFile que se creo en google drive</returns>
        private async Task<string> UploadFile(DriveService service,IFormFile file,string idFolder)
        {

            string fileMime = file.ContentType;
            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = file.Name;
            driveFile.MimeType = file.ContentType;
            driveFile.Parents = new string[] { idFolder };            

            var request = service.Files.Create(driveFile, file.OpenReadStream(),fileMime); //OpenReadStream permite abrir el archivo para enviarlo al servicio de Google Drive

            //request.Fields permite que se generen los campos que queremos obtener informacion como el id, webViewLink, etc. Vease os campos que tiene en el ResponseBody.{Fields}
            request.Fields = "id, webViewLink"; //Se agrego webViewlink para obtener el link de enlace

            
            var response = await request.UploadAsync();

            if (response.Status != UploadStatus.Completed)
            {
                throw response.Exception;
            }            

            return request.ResponseBody.Id;
        }

        /// <summary>
        /// Esta función permite eliminar un archivo con el IdFile que se encuentre en el google drive 
        /// </summary>
        /// <param name="service">Es la sesion que se abrio para enlazar la aplicacion con google drive</param>
        /// <param name="idFile">Es el identificador del archivo a eliminar</param>
        private async void DeleteFile(DriveService service, string idFile)
        {
            var command = service.Files.Delete(idFile);
            var result = await command.ExecuteAsync();
        }


    }

}
