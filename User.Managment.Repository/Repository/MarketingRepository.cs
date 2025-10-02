// <copyright file="MarketingRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class MarketingRepository : Repository<Publicidad>, IMarketingRepository
    {
        private readonly IMapper _mapper;
        protected ResponseDto _response;
        private readonly ApplicationDbContext _db;

        public MarketingRepository(IMapper mapper, ApplicationDbContext db)
            : base(db)
        {
            _db = db;
            _mapper = mapper;
            this._response = new();
        }

        public async Task<ResponseDto> GetAllPublicidad(string? search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var resultQuery = await this.GetAllAsync(u => u.Titulo!.ToLower().Contains(search), tracked: false);
                    _response.Result = _mapper.Map<List<PublicidadDto>>(resultQuery);
                    _response.IsSuccess = true;
                    _response.Message = "Se ha obtenido una lista con la publicidad";
                    _response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    var result = await this.GetAllAsync();
                    _response.Result = _mapper.Map<List<PublicidadDto>>(result);
                    _response.IsSuccess = true;
                    _response.Message = "Se ha obtenido una lista con la publicidad";
                    _response.StatusCode = HttpStatusCode.OK;
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> CreatePublicidad(PublicidadDto publicidadDto)
        {
            try
            {
                if (await this.GetAsync(u => u.Titulo!.ToLower() == publicidadDto.Titulo!.ToLower(), tracked: false) != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro para esta publicidad ya existe";
                }
                else
                {
                    if (publicidadDto.Titulo == "" || publicidadDto.ImageUrl == "")
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "Asegurate de llenar los campos";
                    }
                    else
                    {
                        await this.CreateAsync(_mapper.Map<Publicidad>(publicidadDto));

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.Created;
                        _response.Message = "El registo se ha realizado con éxito";
                    }
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> DeletePublicidad(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    _response.Message = "Ha ocurrido un error. No se pudo eliminar el registro";
                }
                else
                {
                    var publicidad = await this.GetAsync(u => u.Id == id, tracked: false);
                    if (publicidad == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "El registro no existe";
                    }
                    else
                    {
                        await this.RemoveAsync(publicidad);

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "El registro ha sido eliminado";
                    }
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdatetePublicidad(int id, PublicidadDto publicidadDto)
        {
            try
            {
                if (publicidadDto == null || id != publicidadDto.Id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido actualizar el registro";
                }

                _db.PublicidadTbl.Update(_mapper.Map<Publicidad>(publicidadDto));
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha actualizado el registro con éxito";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}