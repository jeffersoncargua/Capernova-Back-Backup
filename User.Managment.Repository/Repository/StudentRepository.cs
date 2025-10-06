// <copyright file="StudentRepository.cs" company="PlaceholderCompany">
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Data.Models.PaypalOrder;
using User.Managment.Data.Models.PaypalOrder.Dto;
using User.Managment.Data.Models.Student;
using User.Managment.Data.Models.Student.DTO;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;
using static Google.Apis.Drive.v3.DriveService;

namespace User.Managment.Repository.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ICourseRepositoty _dbCourse;
        private readonly IMatriculaRepository _dbMatricula;
        private readonly IDeberRepository _dbDeber;
        private readonly ICapituloRepository _dbCapitulo;
        private readonly IPruebaRepository _dbPrueba;
        private readonly IVideoRepository _dbVideo;
        private readonly GoogleDriveSettings _googleDriveConfig;
        protected ResponseDto _response;

        // protected string clientSecret;
        // protected string clientId;
        // protected string authUri;
        public StudentRepository(ApplicationDbContext db, ICourseRepositoty dbCourse, IMatriculaRepository dbMatricula, IDeberRepository dbDeber, ICapituloRepository dbCapitulo, IPruebaRepository dbPrueba, GoogleDriveSettings googleDriveConfig, IMapper mapper, IVideoRepository dbVideo)
            : base(db)
        {
            _db = db;
            _dbCourse = dbCourse;
            _dbMatricula = dbMatricula;
            _dbDeber = dbDeber;
            _dbCapitulo = dbCapitulo;
            _dbPrueba = dbPrueba;
            _mapper = mapper;

            // _configuration = configuration;
            // this.clientId = _configuration["GoogleDrive:ClientId"]; //permite obtener del archivo appsettings.json el clientId de google drive
            // this.clientSecret = _configuration["GoogleDrive:ClientSecret"]; //permite obtener del archivo appsettings.json el redirectUri de google drive
            // this.authUri = _configuration["GoogleDrive:RedirectUri"]; //permite obtener del archivo appsettings.json el redirectUri de ggolge drive
            _googleDriveConfig = googleDriveConfig;
            this._response = new();
            _dbVideo = dbVideo;
        }

        public async Task<ResponseDto> CreateCommentaryAsync(ComentarioDto comentarioDto)
        {
            try
            {
                if (comentarioDto == null || string.IsNullOrEmpty(comentarioDto.FeedBack))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. Recuerda enviarnos tus comentarios y sugerencias";
                }
                else
                {
                    await _db.ComentarioTbl.AddAsync(_mapper.Map<Comentario>(comentarioDto));
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha registrado tu comentario. Muchas Gracias!!";
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

        public async Task<ResponseDto> CreateViewVideoAsync(EstudianteVideoDto estudianteVideoDto)
        {
            try
            {
                if (estudianteVideoDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error!! No se puede registrar la visualización del video por el estudiante!!";
                }
                else
                {
                    var viewExist = await _db.EstudianteVideoTbl.AsNoTracking().FirstOrDefaultAsync(u => u.VideoId == estudianteVideoDto.VideoId && u.StudentId == estudianteVideoDto.StudentId);
                    if (viewExist == null)
                    {
                        EstudianteVideo model = new()
                        {
                            StudentId = estudianteVideoDto.StudentId,
                            VideoId = estudianteVideoDto.VideoId,
                            CursoId = estudianteVideoDto.CursoId,
                            Estado = "Visto",
                        };

                        await _db.EstudianteVideoTbl.AddAsync(model);
                        await _db.SaveChangesAsync();

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Se ha registrado la visualización del video por el estudiante!!";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "El video ya ha sido visto por el estudiante!!";
                    }
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

        public async Task<ResponseDto> GetAllTaskAsync(int? id)
        {
            try
            {
                var deberes = await _dbDeber.GetAllAsync(u => u.CourseId == id, tracked: false);
                if (deberes == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los deberes asignados a este curso!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido los deberes de este curso";
                    _response.Result = _mapper.Map<List<DeberDto>>(deberes);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los deberes asignados a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllTestAsync(int? id)
        {
            try
            {
                var tests = await _dbPrueba.GetAllAsync(u => u.CourseId == id, tracked: false);
                if (tests == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado las pruebas asignados a este curso!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la/s prueba/s de este curso";
                    _response.Result = _mapper.Map<List<PruebaDto>>(tests);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado las pruebas asignados a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetCapitulosAsync(int? id)
        {
            try
            {
                var capitulos = await _dbCapitulo.GetAllAsync(u => u.CourseId == id, tracked: false);
                if (capitulos == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los capitulos asignados a este curso!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el/los capitulo/s de este curso";
                    _response.Result = _mapper.Map<List<CapituloDto>>(capitulos);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los capitulos asignados a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetCertificateAsync(string? studentId, int? cursoId)
        {
            try
            {
                var estudianteExist = await _dbMatricula.GetAsync(u => u.EstudianteId == studentId && u.CursoId == cursoId, tracked: false, includeProperties: "Estudiante,Curso");
                if (estudianteExist == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un problema al intentar descargar el certificado";
                }
                else
                {
                    HtmlToPdf converter = new();

                    string htmlContent = @"<html>";
                    htmlContent += "<body>";
                    htmlContent += "<div style=\"background-image: url('https://lh3.googleusercontent.com/d/1gJz900z5Q8wVUboF4rOe14Q4TwiONGxk'); max-width: 1024px;\">";
                    htmlContent += "<div style='display: flex;flex-direction: column;justify-content: center; align-items: center;'>";
                    htmlContent += "<h1 style='margin-top: 164px; margin-buttom : 0px ;padding: 0;font-size: 48px;font-weight: 600;font-family:'Lato';'>CERTIFICADO DE CAPACITACIÓN</h1>";
                    htmlContent += "<h2 style='margin-top: 1px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px;padding: 0px;font-size: 30px;font-weight: 400;'>POR APROBACIÓN</h2>";
                    htmlContent += "<h3 style='margin-top: 8px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px;padding: 0px;font-size: 20px;font-weight: 400'>ESTE CERTIFICADO SE OTORGA A:</h3>";
                    htmlContent += $"<h1 style='margin-top: 8px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px; padding: 0px; font-size: 48px;font-weight: 500;'>{estudianteExist.Estudiante!.Name + " " + estudianteExist.Estudiante.LastName}</h1>";
                    htmlContent += $"<p style='margin-top: 10px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px;padding: 0px;font-size: 18px;font-weight: 400;text-align: justify;max-width: 800px;line-height: 1.5;'>Por haber cursado todos los niveles de manera satisfactoria y con los más altos estándares de educación brindados por el Centro de Capacitación para Profesionales, Emprendedores e Innovación \"Capernova\", el curso de Experto en {estudianteExist.Curso!.Titulo} con 120 horas de estudio.</p>";
                    htmlContent += "<br><br><br><br><br><br><br>";
                    htmlContent += "</div>";
                    htmlContent += "</div>";
                    htmlContent += "</body>";
                    htmlContent += "</html>";

                    // Margenes del documento
                    converter.Options.MarginTop = 5;
                    converter.Options.MarginRight = 35;
                    converter.Options.MarginBottom = 5;
                    converter.Options.MarginLeft = 35;

                    // ancho del documento
                    converter.Options.WebPageHeight = 1024;

                    // orientacion del documento
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;

                    // convertir el documento html a pdf para enviar al front-end
                    PdfDocument doc = converter.ConvertHtmlString(htmlContent);

                    byte[] pdfFile = doc.Save();

                    doc.Close();

                    FileResult fileResult = new FileContentResult(pdfFile, "application/pdf")
                    {
                        FileDownloadName = "Certificado.pdf",
                    };

                    var service = GetService(); // Se inicia sesion para enviar o eliminar archivos en google drive

                    // En caso de que exista el identificador del archivo se procede a eliminarlo de google drive para poder almacenar otro
                    if (estudianteExist.CertificadoId != null)
                    {
                        DeleteFile(service, estudianteExist.CertificadoId);
                    }

                    // Este es el identificador de la carpeta certificados del drive : 1Cg1aWAsfZW-uZgupiBqKbZ5iGIZXF0aa
                    string idCertificado = await UploadCertificado(service, pdfFile, "1Cg1aWAsfZW-uZgupiBqKbZ5iGIZXF0aa", "application/pdf", estudianteExist.Estudiante.Name!, estudianteExist.Estudiante.LastName!);

                    if (string.IsNullOrEmpty(idCertificado))
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "Ha ocurrido un problema al intentar descargar el certificado";
                        return _response;
                    }

                    Matricula model = new()
                    {
                        Id = estudianteExist.Id,
                        CursoId = estudianteExist.CursoId,
                        EstudianteId = estudianteExist.EstudianteId,
                        IsActive = estudianteExist.IsActive,
                        Estado = estudianteExist.Estado,
                        NotaFinal = estudianteExist.NotaFinal,
                        CertificadoId = idCertificado,
                    };

                    await _dbMatricula.UpdateAsync(model);
                    await _dbMatricula.SaveAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;

                    // _response.Message = "Se ha enviado el certificado";
                    _response.Message = idCertificado;
                    _response.Result = fileResult;
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los capitulos asignados a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetCursoAsync(int id)
        {
            try
            {
                var curso = await _dbCourse.GetAsync(u => u.Id == id);
                if (curso == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error!! No se encuentra registrado el curso";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha enviado el curso del estudiante";
                    _response.Result = _mapper.Map<CourseDto>(curso);
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

        public async Task<ResponseDto> GetCursosAsync(string? id)
        {
            try
            {
                var cursos = await _dbMatricula.GetAllAsync(u => u.EstudianteId == id, tracked: false, includeProperties: "Curso");
                if (cursos == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error!! No se encuentra registrado en ningun curso";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha enviado la lista de cursos del estudiante";
                    _response.Result = _mapper.Map<List<MatriculaDto>>(cursos);
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

        public async Task<ResponseDto> GetGradeTaskAsync(int? id, string studentId)
        {
            try
            {
                var notaDeber = await _db.NotaDeberTbl.AsNoTracking().FirstOrDefaultAsync(u => u.DeberId == id && u.StudentId == studentId);
                if (notaDeber == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado la nota asignado a este deber!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la calificación de este deber";
                    _response.Result = _mapper.Map<NotaDeberDto>(notaDeber);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado la nota de este deber!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetGradeTestAsync(int? id, string studentId)
        {
            try
            {
                var notaPrueba = await _db.NotaPruebaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.PruebaId == id && u.StudentId == studentId);
                if (notaPrueba == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado la nota asignado a esta prueba!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la calificación de esta prueba";
                    _response.Result = _mapper.Map<NotaPruebaDto>(notaPrueba);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado la nota de esta prueba!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetStudentAsync(string? id)
        {
            try
            {
                var estudiante = await this.GetAsync(u => u.Id == id, tracked: false);
                if (estudiante == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error!! No se encuentra registrado";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la información del estudiante";
                    _response.Result = _mapper.Map<StudentDto>(estudiante);
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

        public async Task<ResponseDto> GetVideosAsync(int? id)
        {
            try
            {
                var videos = await _dbVideo.GetAllAsync(u => u.CapituloId == id, tracked: false);
                if (videos == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los videos asignados a este capítulo!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el/los video/s de este capítulo";
                    _response.Result = _mapper.Map<List<VideoDto>>(videos);
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

        public async Task<ResponseDto> GetViewVideosAsync(string? studentId, int cursoId)
        {
            try
            {
                var viewExist = await _db.EstudianteVideoTbl.Where(u => u.StudentId == studentId && u.CursoId == cursoId).ToListAsync();
                if (viewExist == null || studentId! == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No existen registros de la visualización del video por el estudiante!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha enviado los registros de la visualización de los video por el estudiante!!";
                    _response.Result = _mapper.Map<List<EstudianteVideoDto>>(viewExist);
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

        public async Task<ResponseDto> UpdateImageStudentAsync(string id, IFormFile? file)
        {
            try
            {
                var studentDto = await this.GetAsync(u => u.Id == id, tracked: false);
                if (studentDto == null || studentDto.Id != id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido completar la actualización de tu fotografía!";
                    return _response;
                }

                if (file != null)
                {
                    var service = GetService(); // Se inicia sesion para enviar o eliminar archivos en google drive

                    // En caso de que exista el identificador del archivo se procede a eliminarlo de google drive para poder almacenar otro
                    if (studentDto.PhotoUrl != null)
                    {
                        DeleteFile(service, studentDto.PhotoUrl);
                    }

                    // Permite almacenar el idFile creado en google drive para almacenarlo y utilizarlo en la aplicacion
                    // Este link es el identificador de la anterior carpeta para almacenar las fotos de perfil del estudiante : 1-Y_NFEIWjZyZujpMa-chUDc9GRG2fMdC
                    // Este link es el identificador de la nueva carpeta para almacenar las fotos de perfil del estudiante : 1libChFMmx_kjW2Z_wDfa4x6BItruM-Ik
                    // Estos links estan vinculados a las carpetas de Drive de google por lo que se debe revisar su forderId en google drive
                    string idFile = await UploadFile(service, file, "1-Y_NFEIWjZyZujpMa-chUDc9GRG2fMdC");

                    Student modelWithPhoto = new()
                    {
                        Id = studentDto.Id,
                        Name = studentDto.Name,
                        LastName = studentDto.LastName,
                        Phone = studentDto.Phone,
                        Email = studentDto.Email,
                        PhotoUrl = idFile,
                    };

                    _db.StudentTbl.Update(modelWithPhoto);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Tu fotografía se ha actualizado correctamente!";
                    return _response;
                }

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

        public async Task<ResponseDto> UpdateMatriculaAsync(int id, MatriculaDto matriculaDto)
        {
            try
            {
                var matriculaExist = await _dbMatricula.GetAsync(u => u.Id == id, tracked: false);
                if (matriculaExist != null && matriculaDto.Id == id && matriculaDto != null)
                {
                    Matricula model = new()
                    {
                        Id = matriculaExist.Id,
                        CursoId = matriculaExist.CursoId,
                        EstudianteId = matriculaExist.EstudianteId,
                        IsActive = matriculaExist.IsActive,
                        Estado = "Completado",
                        NotaFinal = matriculaExist.NotaFinal,
                        CertificadoId = matriculaExist.CertificadoId,
                    };

                    _db.MatriculaTbl.Update(model);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha actualizado el estado de la matrícula del estudiante!!";
                    _response.Result = model;
                    return _response;
                }

                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha podido actualizar el estado de la matrícula del estudiante!!";
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

        public async Task<ResponseDto> UpdateStudentAsync(string? id, StudentDto studentDto)
        {
            try
            {
                if (studentDto.Id != id || studentDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error en el sistema. Inténtelo nuevamente!";
                    return _response;
                }

                var student = await this.GetAsync(u => u.Id == id, tracked: false);
                if (student == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El usuario no está registrado!";
                    return _response;
                }

                Student model = new()
                {
                    Id = studentDto.Id,
                    Name = studentDto.Name,
                    LastName = studentDto.LastName,
                    Phone = studentDto.Phone,
                    Email = student.Email,
                    PhotoUrl = student.PhotoUrl,
                };

                _db.StudentTbl.Update(model);
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

        public async Task<ResponseDto> UpsertGradeTaskAsync(int id, string studentId, IFormFile? file)
        {
            try
            {
                var notaDeberDto = await _db.NotaDeberTbl.AsNoTracking().FirstOrDefaultAsync(u => u.DeberId == id && u.StudentId == studentId);
                if (notaDeberDto != null)
                {
                    if (file != null)
                    {
                        var service = GetService(); // Se inicia sesion para enviar o eliminar archivos en google drive

                        var curso = await _dbDeber.GetAsync(u => u.Id == id, tracked: false, includeProperties: "Course");

                        // Esta función permite determinar si existe una carpeta donde se pueda almacenar los deberes
                        if (curso == null)
                        {
                            _response.IsSuccess = false;
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.Message = "Se presentó un error al cargar el deber, inténtalo más tarde!";
                            return _response;
                        }

                        // En caso de que exista el identificador del archivo se procede a eliminarlo de google drive para poder almacenar otro
                        if (notaDeberDto.FileUrl != null)
                        {
                            DeleteFile(service, notaDeberDto.FileUrl);
                        }

                        // Permite almacenar el idFile creado en google drive para almacenarlo y utilizarlo en la aplicacion
                        // string idFile = await UploadFile(service, file, "1PuD7eY7zNN1kVs4v0-bD6t9_XDFJfGFa");
                        string idFile = await UploadFile(service, file, curso.Course!.FolderId!);

                        NotaDeber model = new()
                        {
                            Id = notaDeberDto.Id,
                            Estado = "Entregado",
                            Observacion = file.FileName,
                            FileUrl = idFile,
                            StudentId = studentId,
                            Calificacion = notaDeberDto.Calificacion,
                            DeberId = id,
                        };

                        _db.NotaDeberTbl.Update(model);
                        await _db.SaveChangesAsync();

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Su deber ha sido entregado correctamente!";
                        return _response;
                    }

                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha cargado el deber. Inténtelo nuevamente!";
                    return _response;
                }
                else
                {
                    if (file != null)
                    {
                        var service = GetService(); // Se inicia sesion para enviar o eliminar archivos en google drive

                        var curso = await _dbDeber.GetAsync(u => u.Id == id, tracked: false, includeProperties: "Course");

                        // Permite almacenar el idFile creado en google drive para almacenarlo y utilizarlo en la aplicacion
                        string idFile = await UploadFile(service, file, curso.Course!.FolderId!);

                        NotaDeber model = new()
                        {
                            Estado = "Entregado",
                            Observacion = file.FileName,
                            FileUrl = idFile,
                            StudentId = studentId,
                            DeberId = id,
                        };

                        await _db.NotaDeberTbl.AddAsync(model);
                        await _db.SaveChangesAsync();

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "Su deber ha sido entregado correctamente!";
                        return _response;
                    }

                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha cargado el deber. Inténtelo nuevamente!";
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

        /// <summary>
        /// Esta funcion permite cargar un archivo en google drive
        /// Entre estos archivos que se pueden cargar son documentos, fotos y audios, pero no videos.
        /// </summary>
        /// <param name="service">Es la sesion que se abrio para enlazar la aplicacion con google drive.</param>
        /// <param name="fileName">Es el archivo que se va a subir que puede ser foto, documentos o musica.</param>
        /// <param name="idFolder">Es el identificador de la carpeta donde se va a subir el archivo para este caso es la carpeta PruebaCapernova en Google Drive.</param>
        /// <returns>Retorna el IdFile que se creo en google drive.</returns>
        private async Task<string> UploadCertificado(DriveService service, byte[] fileBytes, string idFolder, string contentType, string fileName, string fileLastName)
        {
            string fileMime = contentType;
            var driveFile = new Google.Apis.Drive.v3.Data.File
            {
                // driveFile.Name = file.Name;
                Name = fileName + fileLastName + ".pdf",
                MimeType = contentType,
                Parents = new string[] { idFolder },
            };
            Stream fileStream = new MemoryStream(fileBytes);
            var request = service.Files.Create(driveFile, fileStream, fileMime); // OpenReadStream permite abrir el archivo para enviarlo al servicio de Google Drive

            // request.Fields permite que se generen los campos que queremos obtener informacion como el id, webViewLink, etc. Vease os campos que tiene en el ResponseBody.{Fields}
            request.Fields = "id, webViewLink"; // Se agrego webViewlink para obtener el link de enlace

            var response = await request.UploadAsync();

            return response.Status != UploadStatus.Completed ? throw response.Exception : request.ResponseBody.Id;
        }
    }
}