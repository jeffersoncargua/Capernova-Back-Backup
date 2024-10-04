using Microsoft.AspNetCore.Mvc;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Data.Models.Managment;
using Microsoft.EntityFrameworkCore;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketingController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ApiResponse _response;
        public MarketingController(ApplicationDbContext db)
        {
            _db=db;
            this._response = new();
        }

        [HttpGet]
        [Route("publicidadList")]
        //[ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string? search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var resultQuery = await _db.PublicidadTbl.Where(u => u.Titulo.ToLower().Contains(search)).ToListAsync();
                    _response.Result = resultQuery;
                    _response.isSuccess = true;
                    _response.Message = "Se ha obtenido una lista con la publicidad";
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }

                var result = _db.PublicidadTbl.ToList();
                _response.Result = result;
                _response.isSuccess = true;
                _response.Message = "Se ha obtenido una lista con la publicidad";
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };                
            }

            return _response;
            
        }

        [HttpPost]
        [Route("createPublicidad")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreatePublicidad([FromBody] PublicidadDto model)
        {
            try
            {
                if (_db.PublicidadTbl.FirstOrDefault(u => u.Titulo.ToLower() == model.Titulo.ToLower()) != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro para esta publicidad ya existe";
                    return BadRequest(_response);
                }

                if (model.Titulo == "" || model.ImageUrl == "")
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Asegurate de llenar los campos";
                    return BadRequest(_response);
                }

                Publicidad newModel = new()
                {
                    imageUrl = model.ImageUrl,
                    Titulo = model.Titulo
                };
                _db.PublicidadTbl.Add(newModel);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.Message = "El registo se ha realizado con éxito";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;

        }



        [HttpDelete("deletePublicidad/{id:int}")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    _response.Message = "Ha ocurrido un error. No se pudo eliminar el registro";
                    return BadRequest(_response);
                }
                var publicidad = _db.PublicidadTbl.FirstOrDefault(u => u.Id == id);
                if (publicidad == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe";
                    return BadRequest(_response);
                }
                _db.PublicidadTbl.Remove(publicidad);
                await _db.SaveChangesAsync();
                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "El registro ha sido eliminado";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;           

        }

        [HttpPut("updatePublicidad/{id:int}")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] PublicidadDto model)
        {
            try
            {
                if (model == null || id != model.Id)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "No se ha podido actualizar el registro";
                    return BadRequest(_response);
                }

                Publicidad publicidad = new()
                {
                    Id = model.Id,
                    imageUrl = model.ImageUrl,
                    Titulo = model.Titulo
                };

                _db.PublicidadTbl.Update(publicidad);
                await _db.SaveChangesAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha actualizado el registro con éxito";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

    }
}
