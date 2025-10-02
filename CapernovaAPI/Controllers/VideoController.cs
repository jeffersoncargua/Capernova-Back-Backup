// <copyright file="VideoController.cs" company="PlaceholderCompany">
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
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepository _repoVideo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public VideoController(IVideoRepository repoVideo, IMapper mapper)
        {
            _repoVideo = repoVideo;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet("getAllVideos/{id:int}", Name = "getAllVideos")]
        public async Task<ActionResult<ApiResponse>> GetAllVideos(int? id)
        {
            var result = await _repoVideo.GetAllVideoAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet("getVideo/{id:int}", Name = "getVideo")]
        public async Task<ActionResult<ApiResponse>> GetVideo(int? id)
        {
            var result = await _repoVideo.GetVideoAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost("createVideo")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateVideo([FromBody] VideoDto videoDto)
        {
            var result = await _repoVideo.CreateVideoAsync(videoDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPut("updateVideo/{id:int}", Name = "updateVideo")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateVideo(int id, [FromBody] VideoDto videoDto)
        {
            var result = await _repoVideo.UpdateVideoAsync(id, videoDto);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpDelete("deleteVideo/{id:int}", Name = "deleteVideo")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteVideo(int? id)
        {
            var result = await _repoVideo.DeleteVideoAsync(id);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}