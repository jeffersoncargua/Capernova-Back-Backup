// <copyright file="CapituloRepository.cs" company="PlaceholderCompany">
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
    public class CapituloRepository : Repository<Capitulo>, ICapituloRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public CapituloRepository(ApplicationDbContext db, IMapper mapper)
            : base(db)
        {
            _db = db;
            _mapper = mapper;
            this._response = new();
        }

        public async Task<ResponseDto> CreateCapitulo(CapituloDto capituloDto)
        {
            try
            {
                if (capituloDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo generar el registro";
                }
                else
                {
                    await this.CreateAsync(_mapper.Map<Capitulo>(capituloDto));

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "El capitulo se ha registrado correctamente!!";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo generar el registro del capitulo!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> DeleteCapitulo(int id)
        {
            try
            {
                var capitulo = await this.GetAsync(u => u.Id == id, tracked: false);
                if (capitulo == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo eliminar el registro";
                }
                else
                {
                    await this.RemoveAsync(capitulo);

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se eliminó el registro correctamente";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado registro de este capitulo!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllCapitulos(int? id)
        {
            try
            {
                var capitulos = await this.GetAllAsync(u => u.CourseId == id, tracked: false);
                if (capitulos == null || capitulos.Count == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los capitulos asignados a este curso!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el/los capitulos de este curso";
                    _response.Result = _mapper.Map<List<CapituloDto>>(capitulos);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los capitulos asignados a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetCapitulo(int id)
        {
            try
            {
                var capitulo = await this.GetAsync(u => u.Id == id, tracked: false);
                if (capitulo == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado el capitulo!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el capitulos de este curso";
                    _response.Result = _mapper.Map<CapituloDto>(capitulo);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado el capitulo asignado a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateCapitulo(int id, CapituloDto capituloDto)
        {
            try
            {
                var capitulo = await this.GetAsync(u => u.Id == id, tracked: false);
                if (capituloDto == null || id != capituloDto.Id || capitulo == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha encontrado registros de este capitulo";
                }
                else
                {
                    _db.CapituloTbl.Update(_mapper.Map<Capitulo>(capituloDto));
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha actualizado el capitulo correctamente";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha enconstrado registros de este capitulo!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}