// <copyright file="DeberRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class DeberRepository : Repository<Deber>, IDeberRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public DeberRepository(ApplicationDbContext db, IMapper mapper)
            : base(db)
        {
            _db = db;
            _mapper = mapper;
            this._response = new();
        }

        public async Task<ResponseDto> CreateDeberAsync(DeberDto deber)
        {
            try
            {
                if (deber == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo generar el registro del deber!";
                }
                else
                {
                    if (await this.GetAsync(u => u.Titulo!.ToLower() == deber.Titulo!.ToLower(), tracked: false) != null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "Ya se ha registrado un deber con un titulo similar!!";
                    }
                    else
                    {
                        await this.CreateAsync(_mapper.Map<Deber>(deber));

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "El deber se ha registrado correctamente!!";
                    }
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo generar el registro del deber !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> DeleteDeberAsync(int id)
        {
            try
            {
                var deber = await this.GetAsync(u => u.Id == id, tracked: false);
                if (deber == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo eliminar el registro";
                }
                else
                {
                    await this.RemoveAsync(deber);

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
                _response.Message = "No se han encontrado registro de este deber!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllDeberesAsync(int? id)
        {
            try
            {
                var deberes = await this.GetAllAsync(u => u.CourseId == id, tracked: false);
                if (deberes == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los deberes asignados a este curso!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el/los deber/es de este curso";
                    _response.Result = _mapper.Map<List<DeberDto>>(deberes);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los deberes de este curso !";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetDeberAsync(int id)
        {
            try
            {
                var deber = await this.GetAsync(u => u.Id == id, tracked: false);
                if (deber == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha encontrado el deber!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el deber de este curso";
                    _response.Result = _mapper.Map<DeberDto>(deber);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado el deber!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateDeberAsync(int id, DeberDto deber)
        {
            try
            {
                var deberFromDB = await this.GetAsync(u => u.Id == id, tracked: false);
                if (deber == null || deber.Id != id || deberFromDB == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se encontró el registro del deber";
                }
                else
                {
                    _db.DeberTbl.Update(_mapper.Map<Deber>(deber));
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "El deber se ha actualizado correctamente!!";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo actualizar el registro del deber !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}