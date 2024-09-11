using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System.ComponentModel;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Ventas;
using static Google.Apis.Drive.v3.DriveService;
using User.Managment.Data.Models.Student.DTO;
using User.Managment.Data.Models.Student;
using User.Managment.Repository.Repository.IRepository;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.PaypalOrder.Dto;
using User.Managment.Data.Models.PaypalOrder;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ICourseRepositoty _dbCourse;
        private readonly IMatriculaRepository _dbMatricula;
        protected ApiResponse _response;
        protected string clientSecret;
        protected string clientId;
        protected string authUri;
        public StudentController(ApplicationDbContext db, IConfiguration configuration, ICourseRepositoty dbCourse, IMatriculaRepository dbMatricula)
        {
            _db=db;
            _dbCourse = dbCourse;
            _dbMatricula = dbMatricula;
            _configuration = configuration;
            this.clientId = _configuration["GoogleDrive:ClientId"]; //permite obtener del archivo appsettings.json el clientId de google drive
            this.clientSecret = _configuration["GoogleDrive:ClientSecret"]; //permite obtener del archivo appsettings.json el redirectUri de google drive
            this.authUri = _configuration["GoogleDrive:RedirectUri"]; //permite obtener del archivo appsettings.json el redirectUri de ggolge drive
            this._response = new();
        }

        [HttpGet]
        [Route("getEstudiante")]
        public async Task<ActionResult<ApiResponse>> GetEstudiante([FromQuery] string? id)
        {
            try
            {
                var estudiante = await _db.StudentTbl.FirstOrDefaultAsync(u => u.Id == id);
                if (estudiante == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error!! No se encuentra registrado";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha la información del estudiante";
                _response.Result = estudiante;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("updateStudent", Name = "updateStudent")]
        public async Task<ActionResult<ApiResponse>> UpdateStudent([FromQuery] string id, [FromBody] StudentDto studentDto)
        {
            try
            {
                if (studentDto.Id != id || studentDto == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error en el sistema, intentelo nuevamente!";
                    return BadRequest(_response);
                }


                var student = await _db.StudentTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (student == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El usuario no esta registrado!";
                    return BadRequest(_response);
                }



                Student model = new()
                {
                    Id = studentDto.Id,
                    Name = studentDto.Name,
                    LastName = studentDto.LastName,
                    Phone = studentDto.Phone,                    
                    Email = student.Email,
                    PhotoUrl = studentDto.Photo
                };

                _db.StudentTbl.Update(model);
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

        [HttpPut("updateImageStudent", Name = "updateImageStudent")]
        //[GoogleScopedAuthorize(DriveService.ScopeConstants.DriveReadonly)]
        public async Task<ActionResult<ApiResponse>> UpdateImageStudent([FromQuery] string id, IFormFile? file)
        {
            try
            {
                var studentDto = await _db.StudentTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (studentDto == null || studentDto.Id != id)
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
                    if (studentDto!.PhotoUrl != null)
                    {
                        DeleteFile(service, studentDto.PhotoUrl);
                    }

                    //Permite almacenar el idFile creado en google drive para almacenarlo y utilizarlo en la aplicacion  
                    string idFile = await UploadFile(service, file, "1PuD7eY7zNN1kVs4v0-bD6t9_XDFJfGFa");

                    Student modelWithPhoto = new()
                    {
                        Id = studentDto.Id,
                        Name = studentDto.Name,
                        LastName = studentDto.LastName,
                        Phone = studentDto.Phone,
                        Email = studentDto.Email,
                        PhotoUrl = idFile
                    };

                    _db.StudentTbl.Update(modelWithPhoto);
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

        
        [HttpGet]
        [Route("getCursos")]
        public async Task<ActionResult<ApiResponse>> GetCursos([FromQuery] string? id)
        {
            try
            {
                //var cursos = await _db.MatriculaTbl.AsNoTracking().Where(u => u.EstudianteId == id).ToListAsync();
                var cursos = await _dbMatricula.GetAllAsync(u => u.EstudianteId == id,tracked:false,includeProperties:"Curso");
                if (cursos == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error!! No se encuentra registrado en ningun curso";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha enviado la lista de cursos del estudiante";
                _response.Result = cursos;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("getCurso/{id:int}", Name = "getCurso")]
        public async Task<ActionResult<ApiResponse>> GetCurso(int id)
        {
            try
            {
                //var cursos = await _db.MatriculaTbl.AsNoTracking().Where(u => u.EstudianteId == id).ToListAsync();
                var curso = await _dbCourse.GetAsync(u => u.Id == id);
                if (curso == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error!! No se encuentra registrado el curso";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha enviado el curso del estudiante";
                _response.Result = curso;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("getCapitulos/{id:int}", Name = "getCapitulos")]
        public async Task<ActionResult<ApiResponse>> GetCapitulos(int? id)
        {
            try
            {
                var capitulos = await _db.CapituloTbl.AsNoTracking().Where(u => u.CourseId == id).ToListAsync();
                if (capitulos == null || capitulos.Count == 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los capitulos asignados a este curso!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el/los capitulos de este curso";
                _response.Result = capitulos;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los capitulos asignados a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("getVideos/{id:int}", Name = "getVideos")]
        public async Task<ActionResult<ApiResponse>> GetVideos(int? id)
        {
            try
            {
                var videos = await _db.VideoTbl.AsNoTracking().Where(u => u.CapituloId == id).ToListAsync();
                if (videos == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los videos asignados a este capitulo!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el/los videos de este capitulo";
                _response.Result = videos;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;                
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [Route("createViewVideo")]
        public async Task<ActionResult<ApiResponse>> CreateViewVideo([FromBody] EstudianteVideoDto estudianteVideoDto) // permite crear la visualuzacion del video por el estudiante
        {
            try
            {
                if (estudianteVideoDto == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error!! No se puede registrar la visualización del video por el estudiante!!";
                    return BadRequest(_response);
                }

                var viewExist = await _db.EstudianteVideoTbl.AsNoTracking().FirstOrDefaultAsync(u => u.VideoId == estudianteVideoDto.VideoId && u.StudentId == estudianteVideoDto.StudentId);
                if (viewExist == null)
                {
                    EstudianteVideo model = new()
                    {
                        StudentId = estudianteVideoDto.StudentId,
                        VideoId = estudianteVideoDto.VideoId,
                        CursoId = estudianteVideoDto.CursoId,
                        Estado = "Visto"
                    };

                    await _db.EstudianteVideoTbl.AddAsync(model);
                    await _db.SaveChangesAsync();

                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha registrado la visualización del video por el estudiante!!";
                    return Ok(_response);
                }
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Message = "No se pudo registrar la visualización del video por el estudiante!!";
                return BadRequest(_response);

            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }


        [HttpGet]
        [Route("getViewVideos")]
        public async Task<ActionResult<ApiResponse>> GetViewVideos([FromQuery] string? studentId, [FromQuery] int cursoId) // permite crear la visualuzacion del video por el estudiante
        {
            try
            {
                var viewExist = await _db.EstudianteVideoTbl.Where(u => u.StudentId == studentId && u.CursoId == cursoId).ToListAsync();
                if (viewExist == null || studentId! == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No existen registros de la visualización del video por el estudiante!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha enviado el registrado de la visualización del video por el estudiante!!";
                _response.Result = viewExist;
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("updateMatricula/{id:int}", Name = "updateMatricula")]
        public async Task<ActionResult<ApiResponse>> UpdateMatricula(int id, [FromBody] MatriculaDto matriculaDto)
        {
            try
            {
                var matriculaExist = await _db.MatriculaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if ( matriculaExist !=null  && matriculaDto.Id == id && matriculaDto != null)
                {
                    Matricula model = new()
                    {
                        Id = matriculaExist.Id,
                        CursoId = matriculaExist.CursoId,
                        EstudianteId = matriculaExist.EstudianteId,
                        IsActive = matriculaExist.IsActive,
                        Estado = "Completado",
                        NotaFinal= matriculaExist.NotaFinal
                    };

                    _db.MatriculaTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha actualizado el estado de la matricula del estudiante!!";   
                    _response.Result = model;
                    return Ok(_response);
                }

                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha podido actualizar el estado de la matricula del estudiante!!";
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }


        [HttpGet("getDeberes", Name = "getDeberes")]
        public async Task<ActionResult<ApiResponse>> GetDeberes([FromQuery] int? id)
        {
            try
            {
                var deberes = await _db.DeberTbl.AsNoTracking().Where(u => u.CourseId == id).ToListAsync();
                if (deberes == null || deberes.Count == 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los deberes asignados a este curso!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido los deberes de este curso";
                _response.Result = deberes;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los deberes asignados a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("getNotaDeber", Name = "getNotaDeber")]
        public async Task<ActionResult<ApiResponse>> GetNotaDeber([FromQuery] int? id, [FromQuery] string studentId)
        {
            try
            {
                var notaDeber = await _db.NotaDeberTbl.AsNoTracking().FirstOrDefaultAsync(u => u.DeberId == id && u.StudentId==studentId);
                if (notaDeber == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado la nota asignado a este deber!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la calificación de este deber";
                _response.Result = notaDeber;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado la nota de este deber!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("upsertNotaDeber", Name = "upsertNotaDeber")]
        //[GoogleScopedAuthorize(DriveService.ScopeConstants.DriveReadonly)]
        public async Task<ActionResult<ApiResponse>> UpsertNotaDeber([FromQuery] int id, [FromQuery] string studentId, IFormFile? file)
        {
            try
            {
                var notaDeberDto = await _db.NotaDeberTbl.AsNoTracking().FirstOrDefaultAsync(u => u.DeberId == id && u.StudentId==studentId);
                if (notaDeberDto != null)
                {
                    if (file != null)
                    {
                        var service = GetService(); //Se inicia sesion para enviar o eliminar archivos en google drive

                        //En caso de que exista el identificador del archivo se procede a eliminarlo de google drive para poder almacenar otro
                        if (notaDeberDto.FileUrl != null)
                        {
                            DeleteFile(service, notaDeberDto.FileUrl);
                        }

                        //Permite almacenar el idFile creado en google drive para almacenarlo y utilizarlo en la aplicacion  
                        string idFile = await UploadFile(service, file, "1PuD7eY7zNN1kVs4v0-bD6t9_XDFJfGFa");


                        NotaDeber model = new()
                        {
                            Id = notaDeberDto.Id,
                            Estado = "Entregado",
                            Observacion = file.FileName,
                            FileUrl = idFile,
                            StudentId = studentId,
                            Calificacion = notaDeberDto.Calificacion,
                            DeberId = id
                        };

                        //await _db.NotaDeberTbl.AddAsync(model);
                        _db.NotaDeberTbl.Update(model);
                        await _db.SaveChangesAsync();

                        _response.isSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Su deber ha sido entregado correctamente!";
                        return Ok(_response);

                    }

                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha cargado el deber. Inténtelo nuevamente!";
                    return BadRequest(_response);
                }
                else
                {
                    if (file != null)
                    {
                        var service = GetService(); //Se inicia sesion para enviar o eliminar archivos en google drive

                        //Permite almacenar el idFile creado en google drive para almacenarlo y utilizarlo en la aplicacion  
                        string idFile = await UploadFile(service, file, "1PuD7eY7zNN1kVs4v0-bD6t9_XDFJfGFa");


                        NotaDeber model = new()
                        {
                            Estado = "Entregado",
                            Observacion = file.FileName,
                            FileUrl = idFile,
                            StudentId = studentId,
                            DeberId = id
                        };

                        await _db.NotaDeberTbl.AddAsync(model);
                        //_db.NotaDeberTbl.Update(model);
                        await _db.SaveChangesAsync();

                        _response.isSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Su deber ha sido entregado correctamente!";
                        return Ok(_response);

                    }

                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha cargado el deber. Inténtelo nuevamente!";
                    return BadRequest(_response);
                }

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


        [HttpGet("getPruebas", Name = "getPruebas")]
        public async Task<ActionResult<ApiResponse>> GetPruebas([FromQuery] int? id)
        {
            try
            {
                var deberes = await _db.PruebaTbl.AsNoTracking().Where(u => u.CourseId == id).ToListAsync();
                if (deberes == null || deberes.Count == 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado las pruebas asignados a este curso!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido las pruebas de este curso";
                _response.Result = deberes;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado las pruebas asignados a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }


        [HttpGet("getNotaPrueba", Name = "getNotaPrueba")]
        public async Task<ActionResult<ApiResponse>> GetNotaPrueba([FromQuery] int? id, [FromQuery] string studentId)
        {
            try
            {
                var notaDeber = await _db.NotaPruebaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.PruebaId == id && u.StudentId == studentId);
                if (notaDeber == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado la nota asignado a esta prueba!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la calificación de esta prueba";
                _response.Result = notaDeber;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado la nota de esta prueba!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }


        [HttpPost]
        [Route("createComentario")]
        public async Task<ActionResult<ApiResponse>> CreateComentario([FromBody] ComentarioDto comentarioDto)
        {
            try
            {
                if(comentarioDto == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido registar su comentario";
                    return BadRequest(_response);
                }

                Comentario model = new()
                {
                    Name = comentarioDto.Name,
                    LastName = comentarioDto.LastName,
                    PhotoUrl = comentarioDto.PhotoUrl,
                    FeedBack = comentarioDto.FeedBack,
                    Titulo = comentarioDto.Titulo
                };

                await _db.ComentarioTbl.AddAsync(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha registrado su comentario. Muchas Gracias!!";
                _response.Result = model;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("getCertificate/{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetCertificate(int id)
        {
            var course = await _db.CourseTbl.FirstOrDefaultAsync(u => u.Id ==id);
            if (course == null)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "Ha ocurrido un problema al intentar descargar el certificado";
                return BadRequest(_response);
            }

            HtmlToPdf converter = new HtmlToPdf();

            //string htmlContent = @"<html>";
            //htmlContent += "<body>";
            string htmlContent = "<div style='background-image: url(\"https://i.postimg.cc/SK55dgbd/certificado-Capernova.jpg\"); max-width: 1024px;'>";
            htmlContent += "<div style='display: flex;flex-direction: column;justify-content: center; align-items: center;'>";
            htmlContent += "<h1 style='margin-top: 160px; margin-buttom : 0px ;padding: 0;font-size: 48px;font-weight: 600;font-family:'Lato';'>CERTIFICADO DE CAPACITACIÓN</h1>";
            htmlContent += "<h2 style='margin-top: 1px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px;padding: 0px;font-size: 30px;font-weight: 400;'>POR APROBACIÓN</h2>";
            htmlContent += "<h3 style='margin-top: 1px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px;padding: 0px;font-size: 20px;font-weight: 400'>ESTE CERTIFICADO SE OTORGA A:</h3>";
            htmlContent += "<h1 style='margin-top: 1px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px; padding: 0px; font-size: 48px;font-weight: 500;'>César Almachi</h1>";
            htmlContent += $"<p style='margin-top: 1px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px;padding: 0px;font-size: 18px;font-weight: 400;text-align: justify;max-width: 800px;line-height: 1.5;'>Por haber cursado todos los niveles de manera satisfactoria y con los más altos estándares de educación brindados por el Centro de Capacitación para Profesionales, Emprendedores e Innovación \"Capernova\", el curso de Experto en {course.Titulo} con Técnica Franceso con 120 horas de estudio.</p>";
            htmlContent += "<br><br><br><br><br><br><br><br>";
            htmlContent += "</div>";
            htmlContent += "</div>";
            //htmlContent += "</body>";
            //htmlContent += "</html>";


            //Margenes del documento
            converter.Options.MarginTop = 5;
            converter.Options.MarginLeft = 35;
            converter.Options.MarginTop = 5;
            converter.Options.MarginTop = 5;

            //doc.Margins.Top = 5;
            //doc.Margins.Right = 10;
            //doc.Margins.Bottom = 10;
            //doc.Margins.Right = 35;

            //ancho del documento
            converter.Options.WebPageHeight = 1024;

            //orientacion del documento
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;

            //convertir el documento html a pdf
            PdfDocument doc = converter.ConvertHtmlString(htmlContent);

            byte[] pdfFile = doc.Save();

            doc.Close();

            FileResult fileResult = new FileContentResult(pdfFile, "application/pdf");
            fileResult.FileDownloadName = "Certificado.pdf";

            _response.isSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Message = "Se ha enviado el certificado";
            _response.Result = fileResult;
            return Ok(_response);

        }

        #region Funciones Drive

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
        private async Task<string> UploadFile(DriveService service, IFormFile file, string idFolder)
        {

            string fileMime = file.ContentType;
            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = file.Name;
            driveFile.MimeType = file.ContentType;
            driveFile.Parents = new string[] { idFolder };

            var request = service.Files.Create(driveFile, file.OpenReadStream(), fileMime); //OpenReadStream permite abrir el archivo para enviarlo al servicio de Google Drive

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


        #endregion
    }
}
