// <copyright file="PruebaController.cs" company="PlaceholderCompany">
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
    public class PruebaController : ControllerBase
    {
        private readonly IPruebaRepository _respoPrueba;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public PruebaController(IPruebaRepository respoPrueba, IMapper mapper)
        {
            _respoPrueba = respoPrueba;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet("getAllPruebas/{id:int}", Name = "getAllPruebas")]
        public async Task<ActionResult<ApiResponse>> GetAllPruebas(int? id)
        {
            var result = await _respoPrueba.GetAllTest(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getPrueba/{id:int}", Name = "getPrueba")]
        public async Task<ActionResult<ApiResponse>> GetPrueba(int? id)
        {
            var result = await _respoPrueba.GetTest(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost("createPrueba")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreatePrueba([FromBody] PruebaDto pruebaDto)
        {
            var result = await _respoPrueba.CreateTest(pruebaDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updatePrueba/{id:int}", Name = "updatePrueba")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdatePrueba(int id, [FromBody] PruebaDto pruebaDto)
        {
            var result = await _respoPrueba.UpdateTest(id, pruebaDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete("deletePrueba/{id:int}", Name = "deletePrueba")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeletePrueba(int id)
        {
            var result = await _respoPrueba.DeleteTest(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}