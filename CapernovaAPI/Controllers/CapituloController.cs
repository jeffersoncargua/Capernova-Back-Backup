// <copyright file="CapituloController.cs" company="PlaceholderCompany">
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
    public class CapituloController : ControllerBase
    {
        private readonly ICapituloRepository _repoCapitulo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public CapituloController(ICapituloRepository repoCapitulo, IMapper mapper)
        {
            _repoCapitulo = repoCapitulo;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet("getAllCapitulo/{id:int}", Name = "getAllCapitulo")]
        public async Task<ActionResult<ApiResponse>> GetAllCapitulo(int id)
        {
            var result = await _repoCapitulo.GetAllCapitulos(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getCapitulo/{id:int}", Name = "getCapitulo")]
        public async Task<ActionResult<ApiResponse>> GetCapitulo(int id)
        {
            var result = await _repoCapitulo.GetCapitulo(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost("createCapitulo")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateCapitulo([FromBody] CapituloDto capituloDto)
        {
            var result = await _repoCapitulo.CreateCapitulo(capituloDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateCapitulo/{id:int}", Name = "updateCapitulo")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateCapitulo(int id, [FromBody] CapituloDto capituloDto)
        {
            var result = await _repoCapitulo.UpdateCapitulo(id, capituloDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete("deleteCapitulo/{id:int}", Name = "deleteCapitulo")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteCapitulo(int id)
        {
            var result = await _repoCapitulo.DeleteCapitulo(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}