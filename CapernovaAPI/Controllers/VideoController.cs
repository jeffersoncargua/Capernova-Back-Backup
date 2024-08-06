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
    public class VideoController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ApiResponse _response;
        public VideoController(ApplicationDbContext db)
        {
            _db = db;
            this._response = new();
        }


        [HttpGet("getAllVideos/{id:int}", Name = "getAllVideos")]
        public async Task<ActionResult<ApiResponse>> GetAllVideos(int? id)
        {
            try
            {
                var videos = await _db.VideoTbl.AsNoTracking().Where(u => u.CapituloId == id).ToListAsync();
                if (videos == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado los videos asignados a este capitulo!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el/los capitulos de este capitulo";
                _response.Result = videos;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado los videos de este capitulos !";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("getVideo/{id:int}", Name = "getVideo")]
        public async Task<ActionResult<ApiResponse>> GetVideo(int? id)
        {
            try
            {
                var video = await _db.VideoTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (video == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se han encontrado el video!!";
                    return BadRequest(_response);
                }

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido el video de este capitulo";
                _response.Result = video;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se han encontrado el video!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPost("createVideo")]
        public async Task<ActionResult<ApiResponse>> CreateVideo([FromBody] VideoDto videoDto)
        {
            try
            {
                if (videoDto == null )
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo generar el registro";
                    return BadRequest(_response);
                }

                Video model = new()
                {
                    Titulo = videoDto.Titulo,
                    CapituloId = videoDto.CapituloId,
                    VideoUrl = videoDto.VideoUrl,
                    OrdenReproduccion = videoDto.OrdenReproduccion,

                };

                _db.VideoTbl.Add(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El video se ha registrado correctamente!!";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo generar el registro del video !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("updateVideo/{id:int}",Name = "updateVideo")]
        public async Task<ActionResult<ApiResponse>> UpdateVideo(int id, [FromBody] VideoDto videoDto)
        {
            try
            {
                var video = await _db.VideoTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (videoDto == null || videoDto.Id !=id || video == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se encontró el registro";
                    return BadRequest(_response);
                }

                Video model = new()
                {
                    Id = videoDto.Id,
                    Titulo = videoDto.Titulo,
                    CapituloId = videoDto.CapituloId,
                    VideoUrl = videoDto.VideoUrl,
                    OrdenReproduccion = videoDto.OrdenReproduccion,

                };

                _db.VideoTbl.Update(model);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El video se ha actualizado correctamente!!";
                return Ok(_response);


            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "No se pudo actualizar el registro del video !!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }


        [HttpDelete("deleteVideo/{id:int}", Name = "deleteVideo")]
        public async Task<ActionResult<ApiResponse>> DeleteVideo(int? id)
        {
            try
            {
                var video = await _db.VideoTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (video == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error. No se pudo eliminar el registro";
                }

                _db.VideoTbl.Remove(video);
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
                _response.Message = "No se han encontrado registro de este video!!";
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}
