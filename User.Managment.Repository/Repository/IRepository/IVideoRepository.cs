// <copyright file="IVideoRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<ResponseDto> GetAllVideoAsync(int? id);

        Task<ResponseDto> GetVideoAsync(int? id);

        Task<ResponseDto> CreateVideoAsync(VideoDto videoDto);

        Task<ResponseDto> UpdateVideoAsync(int id, VideoDto videoDto);

        Task<ResponseDto> DeleteVideoAsync(int? id);
    }
}