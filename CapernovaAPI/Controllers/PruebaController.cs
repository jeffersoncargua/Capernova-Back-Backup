using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Data.Models.Course;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    public class PruebaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ApiResponse _response;
        public PruebaController(ApplicationDbContext db)
        {
            _db = db;
            this._response = new();
        }

        [HttpGet("getAllPruebas/{id:int}", Name = "getAllPruebas")]
        public async Task<ActionResult<ApiResponse>> GetAllPruebas(int? id)
        {
            try
            {
                var pruebas = await _db.PruebaTbl.AsNoTracking().Where(u => u.CourseId == id).ToListAsync();
                if (pruebas == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado las pruebas asignados a este curso!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el/los prueba/s de este curso";
                _response.Result = pruebas;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado las pruebas de este curso !";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("getPrueba/{id:int}", Name = "getPrueba")]
        public async Task<ActionResult<ApiResponse>> GetPrueba(int? id)
        {
            try
            {
                var prueba = await _db.PruebaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (prueba == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha encontrado el prueba!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la prueba de este curso";
                _response.Result = prueba;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado el prueba!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPost("createPrueba")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreatePrueba([FromBody] PruebaDto pruebaDto)
        {
            try
            {
                if (pruebaDto == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo generar el registro";
                    return BadRequest(_response);
                }

                if (await _db.PruebaTbl.AsNoTracking().FirstOrDefaultAsync(u =>  u.Titulo.ToLower() == pruebaDto.Titulo.ToLower()) != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ya se ha registrado una prueba con un titulo similar!!";
                    return BadRequest(_response);
                }

                Prueba model = new()
                {
                    Titulo = pruebaDto.Titulo,
                    CourseId = pruebaDto.CourseId,
                    Detalle = pruebaDto.Detalle,
                    Test = pruebaDto.Test
                };

                _db.PruebaTbl.Add(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "La prueba se ha registrado correctamente!!";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo generar el registro de la prueba !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("updatePrueba/{id:int}",Name = "updatePrueba")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdatePrueba(int id, [FromBody] PruebaDto pruebaDto)
        {
            try
            {
                var prueba = await _db.PruebaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (prueba == null || pruebaDto.Id != id || prueba == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se encontró el registro";
                    return BadRequest(_response);
                }


                Prueba model = new()
                {
                    Id = pruebaDto.Id,
                    Titulo = pruebaDto.Titulo,
                    CourseId = pruebaDto.CourseId,
                    Detalle = pruebaDto.Detalle,
                    Test = pruebaDto.Test
                };

                _db.PruebaTbl.Update(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "La prueba se ha actualizado correctamente!!";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo actualizar el registro de la prueba !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }


        [HttpDelete("deletePrueba/{id:int}", Name = "deletePrueba")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeletePrueba(int? id)
        {
            try
            {
                var prueba = await _db.PruebaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (prueba == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo eliminar el registro";
                }

                _db.PruebaTbl.Remove(prueba!);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se elimino el registro correctamente";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado registro de esta prueba!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}
