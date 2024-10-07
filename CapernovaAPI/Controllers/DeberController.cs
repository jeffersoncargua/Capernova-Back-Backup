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
    [ApiController]
    public class DeberController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ApiResponse _response;
        public DeberController(ApplicationDbContext db)
        {
            _db = db;
            this._response = new();
        }


        [HttpGet("getAllDeberes/{id:int}", Name = "getAllDeberes")]
        public async Task<ActionResult<ApiResponse>> GetAllDeberes(int? id)
        {
            try
            {
                var deberes = await _db.DeberTbl.AsNoTracking().Where(u => u.CourseId == id).ToListAsync();
                if (deberes == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los deberes asignados a este curso!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el/los deber/es de este curso";
                _response.Result = deberes;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los deberes de este curso !";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("getDeber/{id:int}", Name = "getDeber")]
        public async Task<ActionResult<ApiResponse>> GetDeber(int? id)
        {
            try
            {
                var deber = await _db.DeberTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (deber == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha encontrado el deber!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el deber de este curso";
                _response.Result = deber;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado el deber!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPost("createDeber")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateDeber([FromBody] DeberDto deberDto)
        {
            try
            {
                if (deberDto == null )
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo generar el registro del deber!";
                    return BadRequest(_response);
                }

                if (await _db.DeberTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Titulo.ToLower() == deberDto.Titulo.ToLower()) != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ya se ha registrado un deber con un titulo similar!!";
                    return BadRequest(_response);
                }

                Deber model = new()
                {
                    Titulo = deberDto.Titulo,
                    CourseId = deberDto.CourseId,
                    Detalle = deberDto.Detalle
                };

                _db.DeberTbl.Add(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El deber se ha registrado correctamente!!";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo generar el registro del deber !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("updateDeber/{id:int}", Name = "updateDeber")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> UpdateDeber(int id, [FromBody] DeberDto deberDto)
        {
            try
            {
                var deber = await _db.DeberTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (deberDto == null || deberDto.Id != id || deber == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se encontró el registro del deber";
                    return BadRequest(_response);
                }


                Deber model = new()
                {
                    Id = deberDto.Id,
                    Titulo = deberDto.Titulo,
                    CourseId = deberDto.CourseId,
                    Detalle = deberDto.Detalle
                };

                _db.DeberTbl.Update(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El deber se ha actualizado correctamente!!";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo actualizar el registro del deber !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }


        [HttpDelete("deleteDeber/{id:int}", Name = "deleteDeber")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> DeleteDeber(int? id)
        {
            try
            {
                var deber = await _db.DeberTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (deber == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo eliminar el registro";
                }

                _db.DeberTbl.Remove(deber!);
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
                _response.Message = "No se han encontrado registro de este deber!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}
