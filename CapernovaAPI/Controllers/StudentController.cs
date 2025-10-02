// <copyright file="StudentController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using Microsoft.AspNetCore.Mvc;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Data.Models.PaypalOrder.Dto;
using User.Managment.Data.Models.Student.DTO;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repoStudent;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public StudentController(IStudentRepository repoStudent, IMapper mapper)
        {
            _repoStudent = repoStudent;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [Route("getEstudiante")]
        public async Task<ActionResult<ApiResponse>> GetEstudiante([FromQuery] string? id)
        {
            var result = await _repoStudent.GetStudentAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateStudent", Name = "updateStudent")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateStudent([FromQuery] string id, [FromBody] StudentDto studentDto)
        {
            var result = await _repoStudent.UpdateStudentAsync(id, studentDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateImageStudent", Name = "updateImageStudent")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateImageStudent([FromQuery] string id, IFormFile? file)
        {
            var result = await _repoStudent.UpdateImageStudentAsync(id, file);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet]
        [Route("getCursos")]
        public async Task<ActionResult<ApiResponse>> GetCursos([FromQuery] string? id)
        {
            var result = await _repoStudent.GetCursosAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getCurso/{id:int}", Name = "getCurso")]
        public async Task<ActionResult<ApiResponse>> GetCurso(int id)
        {
            var result = await _repoStudent.GetCursoAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getCapitulos/{id:int}", Name = "getCapitulos")]
        public async Task<ActionResult<ApiResponse>> GetCapitulos(int? id)
        {
            var result = await _repoStudent.GetCapitulosAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getVideos/{id:int}", Name = "getVideos")]
        public async Task<ActionResult<ApiResponse>> GetVideos(int? id)
        {
            var result = await _repoStudent.GetVideosAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [Route("createViewVideo")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateViewVideo([FromBody] EstudianteVideoDto estudianteVideoDto) // permite crear la visualuzacion del video por el estudiante
        {
            var result = await _repoStudent.CreateViewVideoAsync(estudianteVideoDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet]
        [Route("getViewVideos")]
        public async Task<ActionResult<ApiResponse>> GetViewVideos([FromQuery] string? studentId, [FromQuery] int cursoId) // permite crear la visualuzacion del video por el estudiante
        {
            var result = await _repoStudent.GetViewVideosAsync(studentId, cursoId);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateMatricula/{id:int}", Name = "updateMatricula")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateMatricula(int id, [FromBody] MatriculaDto matriculaDto)
        {
            var result = await _repoStudent.UpdateMatriculaAsync(id, matriculaDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getDeberes", Name = "getDeberes")]
        public async Task<ActionResult<ApiResponse>> GetDeberes([FromQuery] int? id)
        {
            var result = await _repoStudent.GetAllTaskAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getNotaDeber", Name = "getNotaDeber")]
        public async Task<ActionResult<ApiResponse>> GetNotaDeber([FromQuery] int? id, [FromQuery] string studentId)
        {
            var result = await _repoStudent.GetGradeTaskAsync(id, studentId);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("upsertNotaDeber", Name = "upsertNotaDeber")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpsertNotaDeber([FromQuery] int id, [FromQuery] string studentId, IFormFile? file)
        {
            var result = await _repoStudent.UpsertGradeTaskAsync(id, studentId, file);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getPruebas", Name = "getPruebas")]
        public async Task<ActionResult<ApiResponse>> GetPruebas([FromQuery] int? id)
        {
            var result = await _repoStudent.GetAllTestAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getNotaPrueba", Name = "getNotaPrueba")]
        public async Task<ActionResult<ApiResponse>> GetNotaPrueba([FromQuery] int? id, [FromQuery] string studentId)
        {
            var result = await _repoStudent.GetGradeTestAsync(id, studentId);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [Route("createComentario")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateComentario([FromBody] ComentarioDto comentarioDto)
        {
            var result = await _repoStudent.CreateCommentaryAsync(comentarioDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getCertificate", Name = "getCertificate")]
        public async Task<ActionResult<ApiResponse>> GetCertificate([FromQuery] string? studentId, [FromQuery] int? cursoId)
        {
            var result = await _repoStudent.GetCertificateAsync(studentId, cursoId);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}