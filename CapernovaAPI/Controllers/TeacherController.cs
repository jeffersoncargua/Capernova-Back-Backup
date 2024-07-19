using Microsoft.AspNetCore.Mvc;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ApiResponse _response;

        public TeacherController(ApplicationDbContext db)
        {
            _db = db;
            this._response = new();
        }

        //[HttpGet("getCourse/{email:string}", Name = "getCourse")]
        //public async Task<ApiResponse> GetCourse(string email)
        //{
        //    _response.isSuccess = true;
        //    _response.StatusCode = HttpStatusCode.OK;
        //    _response.Message = "Falta realizar esta prueba";
        //    _response.Result = "";
        //    return Ok(_response);
        //}
    }
}
