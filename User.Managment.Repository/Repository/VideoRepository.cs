// <copyright file="VideoRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public VideoRepository(ApplicationDbContext db, IMapper mapper)
            : base(db)
        {
            _db = db;
            _mapper = mapper;
            this._response = new();
        }

        public async Task<ResponseDto> CreateVideoAsync(VideoDto videoDto)
        {
            try
            {
                if (videoDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo generar el registro";
                    return _response;
                }

                var videoExist = await _db.VideoTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Titulo!.ToLower() == videoDto.Titulo!.ToLower());
                if (videoExist != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro ya existe";
                    return _response;
                }

                await this.CreateAsync(_mapper.Map<Video>(videoDto));

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El video se ha registrado correctamente!!";
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo generar el registro del video !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> DeleteVideoAsync(int? id)
        {
            try
            {
                var video = await this.GetAsync(u => u.Id == id, tracked: false);
                if (video == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo eliminar el registro";
                }
                else
                {
                    await this.RemoveAsync(video);

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se elimino el registro correctamente";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado registro de este video!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllVideoAsync(int? id)
        {
            try
            {
                var videos = await this.GetAllAsync(u => u.CapituloId == id, tracked: false);
                if (videos == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los videos asignados a este capitulo!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el/los videos de este capitulo";
                    _response.Result = _mapper.Map<List<VideoDto>>(videos);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los videos de este capitulos !";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetVideoAsync(int? id)
        {
            try
            {
                var video = await this.GetAsync(u => u.Id == id, tracked: false);
                if (video == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado el video!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el video de este capitulo";
                    _response.Result = _mapper.Map<VideoDto>(video);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado el video!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateVideoAsync(int id, VideoDto videoDto)
        {
            try
            {
                var video = await this.GetAsync(u => u.Id == id, tracked: false);
                if (videoDto == null || videoDto.Id != id || video == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se encontró el registro";
                }
                else
                {
                    _db.VideoTbl.Update(_mapper.Map<Video>(videoDto));
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "El video se ha actualizado correctamente!!";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo actualizar el registro del video !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}