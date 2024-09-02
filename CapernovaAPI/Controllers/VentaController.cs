using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ApiResponse _response;
        public VentaController(ApplicationDbContext db)
        {
            _db = db;
            this._response = new();
        }


        [HttpGet]
        [Route("getAllVentas")]
        public async Task<ActionResult<ApiResponse>> GetAllVentas([FromQuery] string? search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var ventasQuery = await _db.VentaTbl.Where(u => u.UserId.Contains(search) || u.LastName.ToLower().Contains(search)).ToListAsync();
                    _response.isSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de las ventas";
                    _response.Result = ventasQuery;
                    return Ok(_response);
                }
                

                var ventas = await _db.VentaTbl.ToListAsync();

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Se ha obtenido la lista de las ventas";
                _response.Result = ventas;
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
