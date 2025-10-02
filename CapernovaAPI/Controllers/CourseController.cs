// <copyright file="CourseController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using Microsoft.AspNetCore.Mvc;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepositoty _repoCourse;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public CourseController(ICourseRepositoty repoCourse, IMapper mapper)
        {
            _repoCourse = repoCourse;
            _mapper = mapper;
            this._response = new();
        }

        /// <summary>
        /// Este controlador permite obtener todos los cursos registrados hasta el momento en la base de datos.
        /// </summary>
        /// <param name="search">Es el modelo que permite obtener un curso en especifico de acuerdo al titulo del curso.</param>
        /// <returns>Retorna una lista de cursos y en el caso de tener una busqueda especifica retorna un listado con los cursos especificos.</returns>
        [HttpGet]
        [Route("getAllCourse")]
        public async Task<ActionResult<ApiResponse>> GetAllCourses([FromQuery] string? search = null)
        {
            var result = await _repoCourse.GetAllCourses(search);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        /// <summary>
        /// Este controlador permite obtener el curso de acuerdo al id del curso que se desea obtener.
        /// </summary>
        /// <param name="id">Es el que contiene el identificador a comparar para obtener el curso.</param>
        /// <returns>Retorna un curso en especifico, segun lo requiera el usuario.</returns>
        [HttpGet("getCourse/{id:int}", Name = "getCourse")]
        public async Task<ActionResult<ApiResponse>> GetCourse(int id)
        {
            var result = await _repoCourse.GetCourse(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        /// <summary>
        /// Este controlador permite obtener el curso de acuerdo al id del curso que se desea obtener.
        /// </summary>
        /// <param name="id">Es el que contiene el identificador a comparar para obtener el curso.</param>
        /// <param name="codigo">Es el codigo del producto.</param>
        /// <returns>Restorna un curso en especifico, segun su codigo.</returns>
        [HttpGet("getCourseCode", Name = "getCourseCode")]
        public async Task<ActionResult<ApiResponse>> GetCourseCode([FromQuery] string codigo)
        {
            var result = await _repoCourse.GetCourseCode(codigo);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [Route("createCourse")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateCourse([FromBody] CourseDto course)
        {
            var result = await _repoCourse.CreateCourse(course);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateCourse/{id:int}", Name = "UpdateCourse")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateCourse(int id, [FromBody] CourseDto course)
        {
            var result = await _repoCourse.UpdateAsync(id, course);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete("deleteCourse/{id:int}", Name = "deleteCourse")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteCourse(int id)
        {
            var result = await _repoCourse.DeleteCourse(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}