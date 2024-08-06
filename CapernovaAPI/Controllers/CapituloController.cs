using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapituloController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ApiResponse _response;
        public CapituloController(ApplicationDbContext db)
        {
            _db = db;
            this._response = new();
        }

        [HttpGet("getAllCapitulo/{id:int}", Name = "getAllCapitulo")]
        public async Task<ActionResult<ApiResponse>> GetAllCapitulo(int? id)
        {
            try
            {
                var capitulos = await _db.CapituloTbl.AsNoTracking().Where(u => u.CourseId == id).ToListAsync();
                if (capitulos == null || capitulos.Count == 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los capitulos asignados a este curso!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el/los capitulos de este curso";
                _response.Result = capitulos;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los capitulos asignados a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("getCapitulo/{id:int}" , Name = "getCapitulo")]
        public async Task<ActionResult<ApiResponse>> GetCapitulo(int? id)
        {
            try
            {
                var capitulo = await _db.CapituloTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (capitulo == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado el capitulo!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el capitulos de este curso";
                _response.Result = capitulo;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado el capitulo asignados a este curso!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPost("createCapitulo")]
        public async Task<ActionResult<ApiResponse>> CreateCapitulo([FromBody] CapituloDto capituloDto)
        {
            try
            {
                if (capituloDto == null )
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo generar el registro";
                    return BadRequest(_response);
                }

                Capitulo model = new()
                {
                    Titulo = capituloDto.Titulo,
                    CourseId = capituloDto.CourseId
                };

                _db.CapituloTbl.Add(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El capitulo se ha registrado correctamente!!";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo generar el registro del capitulo!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("updateCapitulo/{id:int}", Name = "updateCapitulo")]
        public async Task<ActionResult<ApiResponse>> UpdateCapitulo(int? id, [FromBody] CapituloDto capituloDto)
        {
            try
            {
                var capitulo = await _db.CapituloTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (capituloDto == null || id != capituloDto.Id || capitulo == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Message = "No se ha encontrado registros de este capitulo";
                    return NotFound(_response);
                }

                Capitulo model = new()
                {
                    Id = capituloDto.Id,
                    Titulo = capituloDto.Titulo,
                    CourseId = capituloDto.CourseId
                };

                _db.CapituloTbl.Update(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha actualizado el capitulo correctamente";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha enconstrado registros de este capitulo!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("deleteCapitulo/{id:int}",Name = "deleteCapitulo")]
        public async Task<ActionResult<ApiResponse>> DeleteCapitulo(int? id)
        {
            try
            {
                var capitulo = await _db.CapituloTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (capitulo == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo eliminar el registro";
                }

                _db.CapituloTbl.Remove(capitulo);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se eliminó el registro correctamente";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se ha encontrado registro de este capitulo!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}
