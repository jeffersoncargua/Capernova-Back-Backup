// <copyright file="PruebaRepository.cs" company="PlaceholderCompany">
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
    public class PruebaRepository : Repository<Prueba>, IPruebaRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public PruebaRepository(ApplicationDbContext db, IMapper mapper)
            : base(db)
        {
            _db = db;
            _mapper = mapper;
            this._response = new();
        }

        public async Task<ResponseDto> CreateTest(PruebaDto pruebaDto)
        {
            try
            {
                if (pruebaDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo generar el registro";
                    return _response;
                }

                if (await this.GetAsync(u => u.Titulo!.ToLower() == pruebaDto.Titulo!.ToLower(), tracked: false) != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ya se ha registrado una prueba con un titulo similar!!";
                }
                else
                {
                    await this.CreateAsync(_mapper.Map<Prueba>(pruebaDto));

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "La prueba se ha registrado correctamente!!";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo generar el registro de la prueba !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> DeleteTest(int id)
        {
            try
            {
                var prueba = await this.GetAsync(u => u.Id == id, tracked: false);
                if (prueba == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo eliminar el registro";
                }
                else
                {
                    await this.RemoveAsync(prueba);

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
                _response.Message = "No se han encontrado registro de esta prueba!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllTest(int? id)
        {
            try
            {
                var pruebas = await this.GetAllAsync(u => u.CourseId == id, tracked: false);
                if (pruebas == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado las pruebas asignados a este curso!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el/los prueba/s de este curso";
                    _response.Result = _mapper.Map<List<PruebaDto>>(pruebas);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado las pruebas de este curso !";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetTest(int? id)
        {
            try
            {
                var prueba = await this.GetAsync(u => u.Id == id, tracked: false);
                if (prueba == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha encontrado el prueba!!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la prueba de este curso";
                    _response.Result = _mapper.Map<PruebaDto>(prueba);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado el prueba!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateTest(int id, PruebaDto pruebaDto)
        {
            try
            {
                var prueba = await this.GetAsync(u => u.Id == id, tracked: false);
                if (prueba == null || pruebaDto.Id != id || prueba == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se encontró el registro";
                }
                else
                {
                    _db.PruebaTbl.Update(_mapper.Map<Prueba>(pruebaDto));
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "La prueba se ha actualizado correctamente!!";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo actualizar el registro de la prueba !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}