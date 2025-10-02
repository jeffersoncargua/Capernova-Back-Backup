// <copyright file="MarketingController.cs" company="PlaceholderCompany">
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
    public class MarketingController : ControllerBase
    {
        private readonly IMarketingRepository _repoMarketing;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public MarketingController(IMarketingRepository repoMarketing, IMapper mapper)
        {
            this._response = new();
            _mapper = mapper;
            _repoMarketing = repoMarketing;
        }

        [HttpGet]
        [Route("publicidadList")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string? search)
        {
            var result = await _repoMarketing.GetAllPublicidad(search);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [Route("createPublicidad")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreatePublicidad([FromBody] PublicidadDto publicidadDto)
        {
            var result = await _repoMarketing.CreatePublicidad(publicidadDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete("deletePublicidad/{id:int}")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            var result = await _repoMarketing.DeletePublicidad(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updatePublicidad/{id:int}")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] PublicidadDto publicidadDto)
        {
            var result = await _repoMarketing.UpdatetePublicidad(id, publicidadDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}