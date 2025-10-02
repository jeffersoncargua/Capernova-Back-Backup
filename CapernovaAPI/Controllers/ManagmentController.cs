// <copyright file="ManagmentController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using Microsoft.AspNetCore.Mvc;
using User.Managment.Data.Models.Authentication.SignUp;
using User.Managment.Data.Models.PaypalOrder.Dto;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagmentController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly IManagmentRepository _repoManagment;
        private readonly IMapper _mapper;

        public ManagmentController(IManagmentRepository repoManagment, IMapper mapper)
        {
            _repoManagment = repoManagment;
            this._response = new();
            _mapper = mapper;
        }

        /// <summary>
        /// Este controlador permite registrar usuarios con los tipos de roles existentes en la base de datos que son: User,Admin,Student,Teacher y Secretary.
        /// </summary>
        /// <param name="registerUser">Es el modelo con la informacion que se recibe del front-end.</param>
        /// <returns>Retorna la respuesta del registro que puede ser satisfactoria o no.</returns>
        [HttpPost]
        [Route("registration")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterUserDto registerUser)
        {
            var result = await _repoManagment.Register(registerUser);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        /// <summary>
        /// Este controlador permite obtener todos los usuarios que se encuentren registrados en la base de datos, donde se realiza operaciones
        /// de Linq para poder obtener la union de las tablas de Users, UserRoles y Roles.
        /// </summary>
        /// <param name="searchRole">Permite buscar un rol en particular dentro de los usuarios con rol registrados.</param>
        /// <param name="searchName">Permite buscar un usuario en particular en base a su nombre y apellido.</param>
        /// <returns>Retorna una lista con los usurios registrados en la base de datos.</returns>
        [HttpGet("getTalent")]
        public async Task<ActionResult<ApiResponse>> GetTalent([FromQuery] string? searchRole, [FromQuery] string? searchName)
        {
            var result = await _repoManagment.GetTalent(searchRole, searchName);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete]
        [Route("deleteUser/{id}", Name = "deleteUser")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteUser(string id)
        {
            var result = await _repoManagment.DeleteUser(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getStudents")]
        public async Task<ActionResult<ApiResponse>> GetStudents([FromQuery] string? search)
        {
            var result = await _repoManagment.GetAllStudent(search);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("assigmentCourse/{id:int}", Name = "AssigmentCourse")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> AssigmentCourse(int id, [FromBody] string teacherId)
        {
            var result = await _repoManagment.AssigmentCourseAsync(id, teacherId);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("deleteAssigmentCourse/{id:int}", Name = "DeleteAssigmentCourse")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteAssigmentCourse(int id, [FromBody] string teacherId)
        {
            var result = await _repoManagment.DeleteAssigmentCourseAsync(id, teacherId);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        /// <summary>
        /// Este controlador permite obtener todos los cursos para poder observar su informacion en el front-end tanto del administrador como el de la secretaria.
        /// </summary>
        /// <param name="search">Es el campo que contiene los caracteres que tengan coincidencia con el titulo del curso.</param>
        /// <returns>Retorna una lista con todos los cursos y en el caso de buscar con search devuelve una lista de acuerdo con el search.</returns>
        [HttpGet]
        [Route("getAllCourse")]
        public async Task<ActionResult<ApiResponse>> GetAllCourse([FromQuery] string? search)
        {
            var result = await _repoManagment.GetAllCourse(search);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet]
        [Route("getComentarios")]
        public async Task<ActionResult<ApiResponse>> GetComentarios([FromQuery] string? search)
        {
            var result = await _repoManagment.GetAllCommentaries(search);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet]
        [Route("getUser")]
        public async Task<ActionResult<ApiResponse>> GetUser([FromQuery] string search)
        {
            var result = await _repoManagment.GetUserAsync(search);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet]
        [Route("getMatricula")]
        public async Task<ActionResult<ApiResponse>> GetMatricula([FromQuery] string userId)
        {
            var result = await _repoManagment.GetMatriculaAsync(userId);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [Route("createMatricula")]
        public async Task<ActionResult<ApiResponse>> CreateMatricula([FromBody] MatriculaDto matriculaDto)
        {
            var result = await _repoManagment.CreateMatriculaAsync(matriculaDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete]
        [Route("deleteMatricula/{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteMatricula(int id)
        {
            var result = await _repoManagment.DeleteMatriculaAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}