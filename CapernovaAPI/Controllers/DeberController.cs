// <copyright file="DeberController.cs" company="PlaceholderCompany">
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
    public class DeberController : ControllerBase
    {
        private readonly IDeberRepository _repoDeber;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public DeberController(IDeberRepository repoDeber, IMapper mapper)
        {
            _repoDeber = repoDeber;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet("getAllDeberes/{id:int}", Name = "getAllDeberes")]
        public async Task<ActionResult<ApiResponse>> GetAllDeberes(int id)
        {
            var result = await _repoDeber.GetAllDeberesAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getDeber/{id:int}", Name = "getDeber")]
        public async Task<ActionResult<ApiResponse>> GetDeber(int id)
        {
            var result = await _repoDeber.GetDeberAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost("createDeber")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateDeber([FromBody] DeberDto deberDto)
        {
            var result = await _repoDeber.CreateDeberAsync(deberDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateDeber/{id:int}", Name = "updateDeber")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateDeber(int id, [FromBody] DeberDto deberDto)
        {
            var result = await _repoDeber.UpdateDeberAsync(id, deberDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete("deleteDeber/{id:int}", Name = "deleteDeber")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteDeber(int id)
        {
            var result = await _repoDeber.DeleteDeberAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}