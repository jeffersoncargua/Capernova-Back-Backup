// <copyright file="TeacherController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using Microsoft.AspNetCore.Mvc;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _repoTeacher;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public TeacherController(ITeacherRepository repoTeacher, IMapper mapper)
        {
            _repoTeacher = repoTeacher;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet("getAllTeacher")]
        public async Task<ActionResult<ApiResponse>> GetAllTeacher()
        {
            var result = await _repoTeacher.GetAllTeacherAsync();

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getTeacher")]
        public async Task<ActionResult<ApiResponse>> GetTeacher([FromQuery] string id)
        {
            var result = await _repoTeacher.GetTeacherAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getAllCourse")]
        public async Task<ActionResult<ApiResponse>> GetAllCourse([FromQuery] string id, [FromQuery] string? search)
        {
            var result = await _repoTeacher.GetAllCourseAsync(id, search);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateTeacher", Name = "updateTeacher")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateTeacher([FromQuery] string id, [FromBody] TeacherDto teacherDto)
        {
            var result = await _repoTeacher.UpdateTeacherAsync(id, teacherDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateImageTeacher", Name = "updateImageTeacher")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateImageTeacher([FromQuery] string id, IFormFile? file)
        {
            var result = await _repoTeacher.UpdateImageTeacherAsync(id, file);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getStudents", Name = "getStudents")]
        public async Task<ActionResult<ApiResponse>> GetStudents([FromQuery] int? cursoId, [FromQuery] string? search, [FromQuery] string start, [FromQuery] string end)
        {
            var result = await _repoTeacher.GetAllStudentAsync(cursoId, search, start, end);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateNotaDeber", Name = "updateNotaDeber")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateNotaDeber([FromQuery] int? id, [FromQuery] string studentId, [FromBody] string? calificacion)
        {
            var result = await _repoTeacher.UpdateGradeTaskAsync(id, studentId, calificacion);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("upsertNotaPrueba", Name = "upsertNotaPrueba")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpsertNotaPrueba([FromQuery] int id, [FromQuery] string studentId, [FromBody] string? calificacion)
        {
            var result = await _repoTeacher.UpsertGradeTestAsync(id, studentId, calificacion);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateMatriculaNota/{id:int}", Name = "updateMatriculaNota")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateMatriculaNota(int id, [FromBody] string notaFinal)
        {
            var result = await _repoTeacher.UpdateGradeMatriculaAsync(id, notaFinal);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateMatriculaEstado/{id:int}/{studentId}", Name = "updateMatriculaEstado")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateMatriculaEstado(int id, string studentId, [FromBody] bool isActive = false)
        {
            var result = await _repoTeacher.UpdateMatriculaStateAsync(id, studentId, isActive);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}